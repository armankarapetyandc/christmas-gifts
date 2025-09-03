using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceMonkey.Scripts.Utilities
{
    public class ChunkSizeCalculator
    {
        private static readonly int[] PossibleSizes = { 128, 256, 512, 1024, 2048, 4096, 8192 };

        private static List<int> GetValidDivisors(int mapSize)
        {
            return PossibleSizes.Where(s => mapSize % s == 0 && s <= mapSize).ToList();
        }

        public static Vector2Int GetBalancedChunkSize(Vector2Int mapSize, float fillPercent)
        {
            fillPercent = Mathf.Clamp01(fillPercent);

            // Interpolate between max and min
            int targetX = Mathf.RoundToInt(Mathf.Lerp(mapSize.x, 128, fillPercent));
            int targetY = Mathf.RoundToInt(Mathf.Lerp(mapSize.y, 128, fillPercent));

            // Snap to nearest lower power-of-two
            int chunkX = Mathf.ClosestPowerOfTwo(targetX);
            int chunkY = Mathf.ClosestPowerOfTwo(targetY);

            // Ensure we don’t exceed map size
            chunkX = Mathf.Min(chunkX, mapSize.x);
            chunkY = Mathf.Min(chunkY, mapSize.y);

            return new Vector2Int(chunkX, chunkY);
        }


        public static Vector2Int GetGridCount(Vector2Int mapSize, Vector2Int chunkSize)
        {
            return new Vector2Int(mapSize.x / chunkSize.x, mapSize.y / chunkSize.y);
        }
    }

    public class MapTesting : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float fillPercent = 0f;
        [SerializeField] private Vector2Int mapSize = new Vector2Int(5624, 7308);
        [SerializeField] private Vector2 mapOrigin = Vector2.zero; // XY origin
        [SerializeField] private int minChunkSize = 128; // smallest chunk allowed

        private void OnDrawGizmos()
        {
            // 1. Compute target chunk size based on fillPercent
            float targetX = Mathf.Lerp(mapSize.x, minChunkSize, fillPercent);
            float targetY = Mathf.Lerp(mapSize.y, minChunkSize, fillPercent);

            // 2. Snap to nearest lower power of two
            int chunkX = Mathf.NextPowerOfTwo(Mathf.FloorToInt(targetX));
            if (chunkX > targetX) chunkX /= 2;

            int chunkY = Mathf.NextPowerOfTwo(Mathf.FloorToInt(targetY));
            if (chunkY > targetY) chunkY /= 2;

            // Optional: keep chunks roughly balanced
            if ((float)chunkX / chunkY > 2f) chunkX = chunkY * 2;
            if ((float)chunkY / chunkX > 2f) chunkY = chunkX * 2;

            Vector2Int chunkSize = new Vector2Int(chunkX, chunkY);

            // 3. Compute grid count
            int gridX = Mathf.CeilToInt((float)mapSize.x / chunkSize.x);
            int gridY = Mathf.CeilToInt((float)mapSize.y / chunkSize.y);

            // 4. Draw each chunk using DrawWireCube
            Gizmos.color = Color.green;
            for (int y = 0; y < gridY; y++)
            {
                for (int x = 0; x < gridX; x++)
                {
                    float posX = mapOrigin.x + x * chunkSize.x + chunkSize.x / 2f;
                    float posY = mapOrigin.y + y * chunkSize.y + chunkSize.y / 2f;

                    Vector3 center = new Vector3(posX, posY, 0f);
                    Vector3 size = new Vector3(chunkSize.x, chunkSize.y, 0.1f); // very thin cube for 2D

                    Gizmos.DrawWireCube(center, size);
                }
            }

            // Optional: label chunk size
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(new Vector3(mapOrigin.x, mapOrigin.y + mapSize.y + 10f, 0f),
                $"Chunk: {chunkSize.x}x{chunkSize.y} | Grid: {gridX}x{gridY}");
#endif
        }
    }
}