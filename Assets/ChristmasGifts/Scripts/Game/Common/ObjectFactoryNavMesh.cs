using ChristmasGifts.Scripts.Game.Tools;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Common
{
    public class ObjectFactoryNavMesh : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private NavMeshSpawner navMeshSpawner;

        public T Create<T>(T prefab, Quaternion rotation) where T : Component
        {
            return navMeshSpawner.TrySpawn(prefab, container, rotation,
                out T instance)
                ? instance
                : null;
        }
    }
}