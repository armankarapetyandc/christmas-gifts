using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class UIParticleEffector
    {
        private static GameObject cachedParticlePrefab;

        // Configuration defaults
        private const string DEFAULT_PREFAB_PATH = "Prefabs/Particles/XPParticle";
        private const int DEFAULT_PARTICLE_COUNT = 1;
        private const float DEFAULT_MOVE_UP_DISTANCE = 500f;
        private const float DEFAULT_DURATION = 1.5f;
        private const float DEFAULT_SPAWN_DELAY = 0.25f;

        public static async UniTask SpawnParticles(
            Vector2 screenPosition,
            Transform parent,
            Canvas canvas = null,
            string prefabPath = DEFAULT_PREFAB_PATH,
            int particleCount = DEFAULT_PARTICLE_COUNT,
            float moveUpDistance = DEFAULT_MOVE_UP_DISTANCE,
            float duration = DEFAULT_DURATION,
            float spawnDelay = DEFAULT_SPAWN_DELAY,
            CancellationToken cancellationToken = default)
        {
            // Load prefab if not cached
            if (cachedParticlePrefab == null)
            {
                cachedParticlePrefab = Resources.Load<GameObject>(prefabPath);
                if (cachedParticlePrefab == null)
                {
                    Debug.LogError($"Failed to load particle prefab from Resources/{prefabPath}");
                    return;
                }
            }

            // Get or find canvas
            if (canvas == null)
            {
                canvas = parent.GetComponentInParent<Canvas>();
                if (canvas == null)
                {
                    Debug.LogError("No Canvas found in parent hierarchy");
                    return;
                }
            }

            // Convert screen position to local point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent as RectTransform,
                screenPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPoint);

            // Spawn particles asynchronously
            await SpawnParticlesAsync(
                localPoint, parent, particleCount, moveUpDistance, duration, spawnDelay, cancellationToken);
        }

        private static async UniTask SpawnParticlesAsync(
            Vector2 atPosition,
            Transform parent,
            int particleCount,
            float moveUpDistance,
            float duration,
            float spawnDelay,
            CancellationToken cancellationToken)
        {
            for (int i = 0; i < particleCount; i++)
            {
                if (cancellationToken.IsCancellationRequested) return;

                var particle = Object.Instantiate(cachedParticlePrefab, parent);
                MoveParticle(particle, atPosition, moveUpDistance, duration, cancellationToken).Forget();

                await UniTask.Delay(
                    System.TimeSpan.FromSeconds(spawnDelay),
                    cancellationToken: cancellationToken);
            }
        }

        private static async UniTask MoveParticle(
            GameObject particle,
            Vector2 atPosition,
            float moveUpDistance,
            float duration,
            CancellationToken cancellationToken)
        {
            RectTransform rt = particle.GetComponent<RectTransform>();
            CanvasGroup cg = particle.GetComponent<CanvasGroup>();
            if (cg == null) cg = particle.AddComponent<CanvasGroup>();

            Vector2 startPos = atPosition;
            float randomX = Random.Range(-30f, 30f);
            Vector2 endPos = startPos + new Vector2(randomX, moveUpDistance);

            float elapsed = 0f;
            float noiseOffset = Random.Range(0f, 100f);

            try
            {
                while (elapsed < duration)
                {
                    if (cancellationToken.IsCancellationRequested || particle == null) break;

                    elapsed += Time.deltaTime;
                    float t = elapsed / duration;

                    float noiseValue = Mathf.PerlinNoise(noiseOffset + t * 2f, Time.time * 0.5f);
                    float distortion = (noiseValue - 0.5f) * 40f;

                    Vector2 currentPos = Vector2.Lerp(startPos, endPos, t);
                    currentPos.x += distortion;

                    rt.anchoredPosition = currentPos;
                    cg.alpha = 1f - t;

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }
            }
            finally
            {
                if (particle != null)
                {
                    Object.Destroy(particle);
                }
            }
        }
    }
}