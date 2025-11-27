using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility
{
   [ExecuteAlways]
    public class CanvasGapRaycast : Image
    {
        public enum HoleShape
        {
            Rectangle,
            Circle
        }

        [SerializeField] private HoleShape holeShape = HoleShape.Rectangle;
        [SerializeField] private Vector2 holeRectSize = new Vector2(100, 100);
        [SerializeField] private float holeRadius = 50f;
        [SerializeField] private Vector2 holeCenter = Vector2.zero;
        [SerializeField] private RectTransform alignToTransform;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            // Get the full rect of this UI element
            Rect rect = rectTransform.rect;
            float minX = rect.xMin;
            float minY = rect.yMin;
            float maxX = rect.xMax;
            float maxY = rect.yMax;

            if (holeShape == HoleShape.Rectangle)
            {
                // Compute hole rectangle in local coordinates
                Rect holeRectLocal = new Rect(holeCenter - holeRectSize * 0.5f, holeRectSize);

                // If the hole is completely inside our rect, draw the "frame" around it:
                // We'll create up to four quads: top, bottom, left, right segments outside the hole.
                // Top segment: from holeRectLocal.yMax to maxY
                if (holeRectLocal.yMax < maxY)
                {
                    AddQuad(vh,
                        new Vector2(minX, holeRectLocal.yMax),
                        new Vector2(maxX, maxY),
                        color);
                }

                // Bottom segment: from minY to holeRectLocal.yMin
                if (holeRectLocal.yMin > minY)
                {
                    AddQuad(vh,
                        new Vector2(minX, minY),
                        new Vector2(maxX, holeRectLocal.yMin),
                        color);
                }

                // Left segment: from minX to holeRectLocal.xMin (restricted by hole vertical bounds)
                if (holeRectLocal.xMin > minX)
                {
                    AddQuad(vh,
                        new Vector2(minX, holeRectLocal.yMin),
                        new Vector2(holeRectLocal.xMin, holeRectLocal.yMax),
                        color);
                }

                // Right segment: from holeRectLocal.xMax to maxX (restricted by hole vertical bounds)
                if (holeRectLocal.xMax < maxX)
                {
                    AddQuad(vh,
                        new Vector2(holeRectLocal.xMax, holeRectLocal.yMin),
                        new Vector2(maxX, holeRectLocal.yMax),
                        color);
                }
            }
            else if (holeShape == HoleShape.Circle)
            {
                // Circle case is complex to draw as a "ring" without polygon triangulation.
                // For simplicity, we will just draw the full rect for now:
                // In a real solution, you'd implement a polygon triangulation to create a donut shape.
                AddQuad(vh,
                    new Vector2(rect.xMin, rect.yMin),
                    new Vector2(rect.xMax, rect.yMax),
                    color);

                // NOTE: The circle hole is not visually represented as a hole here.
                // You would need complex triangulation to carve out the circle from the rectangle.
            }
        }

        private void AddQuad(VertexHelper vh, Vector2 bottomLeft, Vector2 topRight, Color c)
        {
            UIVertex v = UIVertex.simpleVert;
            v.color = c;

            int startIndex = vh.currentVertCount;

            // bottom-left
            v.position = new Vector3(bottomLeft.x, bottomLeft.y, 0);
            vh.AddVert(v);

            // top-left
            v.position = new Vector3(bottomLeft.x, topRight.y, 0);
            vh.AddVert(v);

            // top-right
            v.position = new Vector3(topRight.x, topRight.y, 0);
            vh.AddVert(v);

            // bottom-right
            v.position = new Vector3(topRight.x, bottomLeft.y, 0);
            vh.AddVert(v);

            // Two triangles: (startIndex, startIndex+1, startIndex+2) and (startIndex+2, startIndex+3, startIndex)
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera,
                    out Vector2 localPoint))
            {
                return true;
            }

            switch (holeShape)
            {
                case HoleShape.Rectangle:
                    Rect holeRect = new Rect(holeCenter - (holeRectSize * 0.5f), holeRectSize);
                    if (holeRect.Contains(localPoint))
                        return false; // Allow raycast through hole
                    break;

                case HoleShape.Circle:
                    float distSqr = (localPoint - holeCenter).sqrMagnitude;
                    if (distSqr <= holeRadius * holeRadius)
                        return false; // Allow raycast through hole
                    break;
            }

            return true; // Outside hole, block raycasts
        }

        private void Update()
        {
            // Check pointer inside hole
            Vector2 pointerPos = Input.mousePosition;
            bool insideHole = IsPointerInsideHole(pointerPos);

            // If outside the hole area, ensure no UI remains highlighted/selected
            if (!insideHole && EventSystem.current != null)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        private bool IsPointerInsideHole(Vector2 screenPoint)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null,
                    out Vector2 localPoint))
            {
                switch (holeShape)
                {
                    case HoleShape.Rectangle:
                        Rect holeRect = new Rect(holeCenter - (holeRectSize * 0.5f), holeRectSize);
                        return holeRect.Contains(localPoint);
                    case HoleShape.Circle:
                        float distSqr = (localPoint - holeCenter).sqrMagnitude;
                        return distSqr <= holeRadius * holeRadius;
                }
            }

            return false;
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (alignToTransform != null)
            {
                SetHoleFromUIElement(HoleShape.Rectangle, alignToTransform);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Visualize the hole area in the Scene View
            Gizmos.color = Color.green;

            switch (holeShape)
            {
                case HoleShape.Rectangle:
                {
                    Vector3 size = new Vector3(holeRectSize.x, holeRectSize.y, 0);
                    Matrix4x4 oldMatrix = Gizmos.matrix;
                    Gizmos.matrix = rectTransform.localToWorldMatrix;
                    Gizmos.DrawWireCube(holeCenter, size);
                    Gizmos.matrix = oldMatrix;
                    break;
                }

                case HoleShape.Circle:
                {
                    int segments = 30;
                    float angleStep = 360f / segments;
                    Vector3 holeWorldCenter = rectTransform.TransformPoint(holeCenter);
                    Vector3 prevPoint = holeWorldCenter + (Vector3.right * holeRadius * rectTransform.lossyScale.x);
                    for (int i = 1; i <= segments; i++)
                    {
                        float angle = angleStep * i * Mathf.Deg2Rad;
                        Vector3 newPoint = holeWorldCenter
                                           + (rectTransform.right * Mathf.Cos(angle) * holeRadius *
                                              rectTransform.lossyScale.x)
                                           + (rectTransform.up * Mathf.Sin(angle) * holeRadius *
                                              rectTransform.lossyScale.y);
                        Gizmos.DrawLine(prevPoint, newPoint);
                        prevPoint = newPoint;
                    }

                    break;
                }
            }
        }
#endif

        public void SetHoleFromUIElement(HoleShape shape, RectTransform targetRect)
        {
            if (targetRect == null)
            {
                Debug.LogError("TargetRect is null, unable to point!");
                return;
            }

            holeShape = shape;

            Vector3[] corners = new Vector3[4];
            targetRect.GetWorldCorners(corners);

            for (int i = 0; i < 4; i++)
            {
                corners[i] = rectTransform.InverseTransformPoint(corners[i]);
            }

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            for (int i = 0; i < 4; i++)
            {
                if (corners[i].x < minX) minX = corners[i].x;
                if (corners[i].y < minY) minY = corners[i].y;
                if (corners[i].x > maxX) maxX = corners[i].x;
                if (corners[i].y > maxY) maxY = corners[i].y;
            }

            Vector2 localSize = new Vector2(maxX - minX, maxY - minY);
            Vector2 localCenter = new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);

            holeCenter = localCenter;

            if (shape == HoleShape.Rectangle)
            {
                holeRectSize = localSize;
            }
            else if (shape == HoleShape.Circle)
            {
                holeRadius = Mathf.Min(localSize.x, localSize.y) * 0.5f;
            }
        }

        public void SetHoleFromUIElement(HoleShape shape, params RectTransform[] targetRects)
        {
            if (targetRects == null || targetRects.Length == 0)
            {
                Debug.LogError("No RectTransforms provided, unable to set hole from UI elements.");
                return;
            }

            holeShape = shape;

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            Vector3[] corners = new Vector3[4];

            // Iterate through each RectTransform to find combined bounds
            foreach (var targetRect in targetRects)
            {
                if (targetRect == null) continue;

                targetRect.GetWorldCorners(corners);

                for (int i = 0; i < 4; i++)
                {
                    Vector3 localPoint = rectTransform.InverseTransformPoint(corners[i]);

                    if (localPoint.x < minX) minX = localPoint.x;
                    if (localPoint.y < minY) minY = localPoint.y;
                    if (localPoint.x > maxX) maxX = localPoint.x;
                    if (localPoint.y > maxY) maxY = localPoint.y;
                }
            }

            // If no valid transforms were found, minX etc. might still be at extremes.
            if (minX == float.MaxValue || minY == float.MaxValue || maxX == float.MinValue || maxY == float.MinValue)
            {
                Debug.LogError("No valid RectTransform bounds found.");
                return;
            }

            Vector2 localSize = new Vector2(maxX - minX, maxY - minY);
            Vector2 localCenter = new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);

            holeCenter = localCenter;

            if (shape == HoleShape.Rectangle)
            {
                holeRectSize = localSize;
            }
            else if (shape == HoleShape.Circle)
            {
                holeRadius = Mathf.Min(localSize.x, localSize.y) * 0.5f;
            }
        }
    }
}