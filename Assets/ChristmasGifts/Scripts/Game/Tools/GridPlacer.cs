using System.Collections.Generic;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Tools
{
    public class GridPlacer : MonoBehaviour
    {
        [SerializeField] private Transform objectPrefab; // Prefab 
        [SerializeField] private Transform originPoint; // The transform to use as the origin for placement
        [SerializeField] private int objectCount = 56; // Total objects to place

        [SerializeField]
        private Vector2Int aspectRatio = new Vector2Int(16, 9); // Width and Height aspect ratio (e.g., 16:9)

        [SerializeField] private Vector2 spacing; // Horizontal and vertical spacing between objects

        [SerializeField] private Vector2
            anchorPosition =
                new Vector2(0.5f,
                    0.5f); // Anchor position: (0, 1) for top-left, (0.5, 0.5) for center, (1, 0) for bottom-right

        [SerializeField] private bool useEvenGrid = false; // Whether to enforce an even grid if objectCount is even
        [SerializeField] private bool populateOnStart = false; // Whether to enforce an even grid if objectCount is even

        private void Start()
        {
            if (populateOnStart && objectPrefab != null)
            {
                Populate(objectPrefab);
            }
        }

        public void Populate(Transform prefab)
        {
            // Get the positions for each object in the grid
            List<Vector3> positions = GetPositions();

            // Instantiate objects at each position
            foreach (Vector3 position in positions)
            {
                Instantiate(prefab, position, Quaternion.identity, originPoint);
            }
        }

        public List<Vector3> GetPositions()
        {
            // Calculate grid parameters
            int columns, rows;
            float gridWidth, gridHeight;
            CalculateGridParameters(out columns, out rows, out gridWidth, out gridHeight);

            // Calculate the starting point based on the origin and anchor position
            Vector3 startPoint = CalculateStartPoint(gridWidth, gridHeight);

            // List to store the calculated positions
            List<Vector3> positions = new List<Vector3>();

            int index = 0;
            for (int row = 0; row < rows && index < objectCount; row++)
            {
                for (int col = 0; col < columns && index < objectCount; col++)
                {
                    Vector3 worldPosition = CalculateWorldPosition(startPoint, row, col);
                    positions.Add(worldPosition);
                    index++;
                }
            }

            return positions;
        }

        private void OnDrawGizmos()
        {
            if (originPoint == null) return;

            // Calculate grid parameters
            int columns, rows;
            float gridWidth, gridHeight;
            CalculateGridParameters(out columns, out rows, out gridWidth, out gridHeight);

            // Calculate the starting point based on the origin and anchor position
            Vector3 startPoint = CalculateStartPoint(gridWidth, gridHeight);

            // Define a padding to expand the bounding box slightly
            float padding = 0.5f;

            // Draw grid bounds as a single wire cube aligned to the ground
            DrawGridBounds(startPoint, gridWidth, gridHeight, padding);

            // Draw individual object positions as ground-aligned wire cubes
            List<Vector3> positions = GetPositions();
            Gizmos.color = Color.blue;
            foreach (Vector3 position in positions)
            {
                // Set the Y position to ground level with a 0.25 offset for height
                Vector3 groundAlignedPosition = new Vector3(position.x, 0.25f, position.z);
                Gizmos.DrawWireCube(groundAlignedPosition, Vector3.one * padding); // Adjust size as needed
            }
        }

        private void CalculateGridParameters(out int columns, out int rows, out float gridWidth, out float gridHeight)
        {
            if (useEvenGrid && objectCount % 2 == 0)
            {
                // Make rows and columns equal if objectCount is even and useEvenGrid is true
                columns = rows = Mathf.CeilToInt(Mathf.Sqrt(objectCount));
            }
            else
            {
                // Otherwise, calculate columns and rows based on aspect ratio
                columns = Mathf.CeilToInt(Mathf.Sqrt(objectCount * (float)aspectRatio.x / aspectRatio.y));
                rows = Mathf.CeilToInt((float)objectCount / columns);
            }

            gridWidth = (columns - 1) * spacing.x;
            gridHeight = (rows - 1) * spacing.y;
        }

        private Vector3 CalculateStartPoint(float gridWidth, float gridHeight)
        {
            return originPoint.position + originPoint.rotation * new Vector3(
                -gridWidth * anchorPosition.x,
                0,
                gridHeight * (1 - anchorPosition.y)
            );
        }

        private Vector3 CalculateWorldPosition(Vector3 startPoint, int row, int col)
        {
            float posX = col * spacing.x;
            float posZ = -row * spacing.y;
            Vector3 localPosition = new Vector3(posX, 0, posZ);
            return startPoint + originPoint.rotation * localPosition;
        }

        private void DrawGridBounds(Vector3 startPoint, float gridWidth, float gridHeight, float padding)
        {
            Gizmos.color = Color.green;

            // Calculate the center of the grid bounds at ground level
            Vector3 center = startPoint + originPoint.rotation * new Vector3(gridWidth / 2, 0.25f, -gridHeight / 2);

            // Define the size of the grid bounds with added padding and height of 0.5 for ground alignment
            Vector3 size = new Vector3(gridWidth + padding, 0.5f, gridHeight + padding);

            // Draw a single wire cube that represents the bounds of the grid aligned to the ground
            Gizmos.DrawWireCube(center - new Vector3(0, 0.25f, 0), size); // Adjusted for ground alignment
        }
    }
}