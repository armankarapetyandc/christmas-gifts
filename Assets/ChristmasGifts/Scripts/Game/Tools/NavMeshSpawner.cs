using UnityEngine;
using UnityEngine.AI;

namespace ChristmasGifts.Scripts.Game.Tools
{
    public class NavMeshSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnArea; // The area (with a collider) within which to spawn objects
        [SerializeField] private LayerMask checkLayers; // Layers to check for overlap (e.g., obstacles)
        [SerializeField] private float overlapRadius = 2f; // Radius for overlap check
        [SerializeField] private int maxAttempts = 10; // Max attempts to find a clear spot

        public bool TrySpawn<T>(T prefab, Transform container,Quaternion rotation, out T instance) where T : Component
        {
            if (TryGetRandomSpawnPointInBounds(out Vector3 spawnPoint))
            {
                instance = Instantiate(prefab, container);
                instance.transform.SetPositionAndRotation(spawnPoint, rotation);
                return true;
            }

            instance = null;
            Debug.LogError("Failed to find a valid spawn point after max attempts.");
            return false;
        }

        private bool TryGetRandomSpawnPointInBounds(out Vector3 result)
        {
            // Ensure the spawn area has a collider to define the bounds
            Collider areaCollider = spawnArea.GetComponent<Collider>();
            if (areaCollider == null)
            {
                Debug.LogError("Spawn area requires a collider to define bounds.");
                result = Vector3.zero;
                return false;
            }

            for (int i = 0; i < maxAttempts; i++)
            {
                // Get a random point within the bounds of the collider
                Vector3 randomPoint = GetRandomPointInBounds(areaCollider.bounds);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, Mathf.Infinity, NavMesh.AllAreas))
                {
                    // Check for overlapping objects in the vicinity
                    if (Physics.OverlapSphere(hit.position, overlapRadius, checkLayers).Length == 0)
                    {
                        result = hit.position;
                        return true; // Found a valid point
                    }
                }
            }

            result = Vector3.zero;
            return false; // No valid point found within max attempts
        }

        private Vector3 GetRandomPointInBounds(Bounds bounds)
        {
            // Generate a random point within the bounds
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = bounds.center.y; // Keep it at the center height, or adjust if necessary
            float z = Random.Range(bounds.min.z, bounds.max.z);
            return new Vector3(x, y, z);
        }

        private void OnDrawGizmos()
        {
            if (spawnArea != null)
            {
                // Optional: Draw the bounds of the spawn area in the editor for visualization
                Gizmos.color = Color.red;
                Collider areaCollider = spawnArea.GetComponent<Collider>();
                if (areaCollider != null)
                {
                    Gizmos.DrawWireCube(areaCollider.bounds.center, areaCollider.bounds.size);
                }
            }
        }
    }
}