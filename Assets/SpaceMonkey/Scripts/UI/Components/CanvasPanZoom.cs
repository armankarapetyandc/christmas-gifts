using UIService.Runtime.Presenter;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceMonkey.Scripts.UI.Components
{
    public class CanvasPanZoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("Zoom Settings")] public float minZoom = 0.5f;
        public float maxZoom = 3f;
        public float zoomSpeed = 1f;
        public float touchZoomSpeed = 0.01f;

        [Header("Pan Settings")] public bool enablePanning = true;
        public bool constrainToViewport = true; // Constrain panning to keep image within viewport bounds

        [Header("Smoothing")] public bool enableSmoothing = true;
        public float smoothTime = 0.1f;

        public RectTransform rectTransform;
        public RectTransform viewport; // The parent viewport (Canvas or a container)

        private Vector3 lastMousePosition;
        private Vector3 targetPosition;
        private float targetScale = 1f;
        private float currentScale = 1f;

        // Touch handling
        private Touch[] lastTouches = new Touch[2];
        private float lastTouchDistance;
        private Vector2 lastTouchCenter;
        private bool isDragging = false;

        // Smoothing
        private Vector3 velocity = Vector3.zero;
        private float scaleVelocity = 0f;
        private PresenterView _presenterView;

        [Inject]
        private void Construct(PresenterView presenterView)
        {
            _presenterView = presenterView;
        }

        private void Start()
        {
            targetPosition = rectTransform.anchoredPosition;
            targetScale = rectTransform.localScale.x;
            currentScale = targetScale;
        }

        private void Update()
        {
            if (_presenterView?.Canvas == null) return;

            HandleInput();

            if (enableSmoothing)
            {
                SmoothMovement();
            }
            else
            {
                ApplyTransform();
            }
        }

        public void HandleInput()
        {
            // Handle touch input for mobile
            if (Input.touchCount > 0)
            {
                HandleTouchInput();
            }
            // Handle mouse input for standalone
            else
            {
                HandleMouseInput();
            }
        }

        public void HandleMouseInput()
        {
            if (IsMouseOverGameWindow())
            {
                // Mouse wheel zoom
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f)
                {
                    // Vector2 mousePos;
                    // // For Screen Space - Overlay, use null camera
                    // Camera cam = _presenterView.Canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    //     ? null
                    //     : _presenterView.Canvas.worldCamera;
                    // RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    //     rectTransform, Input.mousePosition, cam, out mousePos);
                    ZoomAtPoint(GetViewportPivotLocal(), scroll * zoomSpeed);
                }

                // Mouse drag is handled by IPointerHandler interfaces
            }
        }

        bool IsMouseOverGameWindow()
        {
#if UNITY_EDITOR
            return EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.titleContent.text == "Game";
#else
    return Application.isFocused;
#endif
        }

        Vector2 GetViewportPivotLocal()
        {
            Vector3 worldPivot = viewport.TransformPoint(viewport.rect.center);
            Vector2 localPivot;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                RectTransformUtility.WorldToScreenPoint(null, worldPivot),
                null,
                out localPivot);
            return localPivot;
        }

        void HandleTouchInput()
        {
            if (Input.touchCount == 1)
            {
                // Single touch - pan
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    lastTouches[0] = touch;
                }
                else if (touch.phase == TouchPhase.Moved && enablePanning)
                {
                    Vector2 deltaPosition = touch.position - lastTouches[0].position;
                    // For Screen Space - Overlay, canvas.scaleFactor is already applied correctly
                    deltaPosition = deltaPosition / _presenterView.Canvas.scaleFactor;

                    targetPosition += (Vector3)deltaPosition;
                    ClampPosition();

                    lastTouches[0] = touch;
                }
            }
            else if (Input.touchCount == 2)
            {
                // Two touch - pinch zoom
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                if (prevTouchDeltaMag > 0)
                {
                    float deltaDistance = touchDeltaMag - prevTouchDeltaMag;

                    // Calculate zoom point (center between two touches)
                    Vector2 touchCenter = (touch1.position + touch2.position) * 0.5f;
                    Vector2 localTouchCenter;
                    // For Screen Space - Overlay, use null camera
                    Camera cam = _presenterView.Canvas.renderMode == RenderMode.ScreenSpaceOverlay
                        ? null
                        : _presenterView.Canvas.worldCamera;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        rectTransform, touchCenter, cam, out localTouchCenter);

                    float zoomAmount = deltaDistance * touchZoomSpeed;
                    ZoomAtPoint(touchCenter, zoomAmount);
                }
            }
        }

        void ZoomAtPoint(Vector2 localPoint, float zoomAmount)
        {
            float previousScale = targetScale;
            targetScale = Mathf.Clamp(targetScale + zoomAmount, minZoom, maxZoom);

            if (Mathf.Approximately(previousScale, targetScale)) return;

            float scaleFactor = targetScale / previousScale;

            // Keep zoom anchor stable
            targetPosition = (Vector3)(((Vector2)targetPosition - localPoint) * scaleFactor + localPoint);

            ClampPosition(); // ✅ make sure we never see blank background
        }

        void ClampPosition()
        {
            if (!constrainToViewport || viewport == null) return;

            // Get world corners of the content
            Vector3[] contentCorners = new Vector3[4];
            rectTransform.GetWorldCorners(contentCorners);

            // Convert to viewport local space
            Vector3[] localCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
                localCorners[i] = viewport.InverseTransformPoint(contentCorners[i]);

            // Get bounds in local space
            Vector2 min = localCorners[0];
            Vector2 max = localCorners[0];
            foreach (var c in localCorners)
            {
                min = Vector2.Min(min, c);
                max = Vector2.Max(max, c);
            }

            // Actual size of content inside viewport’s local space
            Vector2 contentSize = max - min;
            Vector2 viewportSize = viewport.rect.size;

            // Compute clamp limits (half extents)
            float limitX = Mathf.Max(0, (contentSize.x - viewportSize.x) / 2f);
            float limitY = Mathf.Max(0, (contentSize.y - viewportSize.y) / 2f);

            // ✅ If content smaller than viewport, lock to center
            float clampX = (contentSize.x <= viewportSize.x) ? 0 : Mathf.Clamp(targetPosition.x, -limitX, limitX);
            float clampY = (contentSize.y <= viewportSize.y) ? 0 : Mathf.Clamp(targetPosition.y, -limitY, limitY);

            targetPosition = new Vector3(clampX, clampY, targetPosition.z);
        }

        Vector3 SoftClamp(Vector3 pos)
        {
            if (viewport == null) return pos;

            // Get world corners of the content
            Vector3[] contentCorners = new Vector3[4];
            rectTransform.GetWorldCorners(contentCorners);

            Vector3[] localCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
                localCorners[i] = viewport.InverseTransformPoint(contentCorners[i]);

            Vector2 min = localCorners[0];
            Vector2 max = localCorners[0];
            foreach (var c in localCorners)
            {
                min = Vector2.Min(min, c);
                max = Vector2.Max(max, c);
            }

            Vector2 contentSize = max - min;
            Vector2 viewportSize = viewport.rect.size;

            float limitX = Mathf.Max(0, (contentSize.x - viewportSize.x) / 2f);
            float limitY = Mathf.Max(0, (contentSize.y - viewportSize.y) / 2f);

            float x = pos.x;
            float y = pos.y;

            // ✅ Elastic clamp instead of hard lock
            if (contentSize.x <= viewportSize.x)
                x = 0;
            else if (pos.x > limitX)
                x = Mathf.Lerp(pos.x, limitX, 0.5f); // pull back
            else if (pos.x < -limitX)
                x = Mathf.Lerp(pos.x, -limitX, 0.5f);

            if (contentSize.y <= viewportSize.y)
                y = 0;
            else if (pos.y > limitY)
                y = Mathf.Lerp(pos.y, limitY, 0.5f);
            else if (pos.y < -limitY)
                y = Mathf.Lerp(pos.y, -limitY, 0.5f);

            return new Vector3(x, y, pos.z);
        }

        void SmoothMovement()
        {
            // Smooth position with elastic bounce
            targetPosition = SoftClamp(targetPosition);

            rectTransform.anchoredPosition = Vector3.SmoothDamp(
                rectTransform.anchoredPosition, targetPosition, ref velocity, smoothTime);

            // Smooth scale
            currentScale = Mathf.SmoothDamp(currentScale, targetScale, ref scaleVelocity, smoothTime);
            rectTransform.localScale = Vector3.one * currentScale;
        }

        void ApplyTransform()
        {
            rectTransform.anchoredPosition = targetPosition;
            rectTransform.localScale = Vector3.one * targetScale;
            currentScale = targetScale;
        }

        // IPointerHandler implementations for mouse drag
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                lastMousePosition = eventData.position;
                isDragging = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isDragging = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging || !enablePanning) return;

            Vector2 deltaPosition = eventData.position - (Vector2)lastMousePosition;
            // For Screen Space - Overlay, canvas.scaleFactor handles the conversion correctly
            deltaPosition = deltaPosition / _presenterView.Canvas.scaleFactor;

            targetPosition += (Vector3)deltaPosition;
            targetPosition = SoftClamp(targetPosition); // ✅ elastic drag

            lastMousePosition = eventData.position;
        }

        // Public methods for external control
        public void ResetTransform()
        {
            targetPosition = Vector3.zero;
            targetScale = 1f;
        }

        public void SetZoom(float zoom)
        {
            targetScale = Mathf.Clamp(zoom, minZoom, maxZoom);
        }

        public void SetPosition(Vector2 position)
        {
            targetPosition = position;
            targetPosition = SoftClamp(targetPosition); // ✅ elastic drag
        }

        [ContextMenu("print")]
        public void printc()
        {
            Debug.LogError(targetPosition);
        }

        public Vector2 seeeettl;
        [ContextMenu("set")]
        public void sett()
        {
            SetPosition(seeeettl);
        }


        public RectTransform target;

        [ContextMenu("set to target")]
        public void settotarget()
        {
            NavigateToTarget(target);
        }

        public void NavigateToTarget(RectTransform placeItemRect)
        {
            var targetPosition = CalculateTargetPosition(placeItemRect.anchoredPosition);
            SetPosition(targetPosition);
        }

        private Vector2 CalculateTargetPosition(Vector2 position)
        {
            var targetAnchoredPos = position;
            var targetSizeDelta = target.sizeDelta;
            var contentSize = rectTransform.sizeDelta;
            var calculatedPosition = new Vector2(targetAnchoredPos.x - contentSize.x * 0.5f + targetSizeDelta.x * 0.5f,
                targetAnchoredPos.y + contentSize.y * 0.5f - targetSizeDelta.y * 0.5f);
            calculatedPosition *= currentScale * -1f;
            return calculatedPosition;
        }

        public float GetCurrentZoom()
        {
            return currentScale;
        }

        public Vector2 GetCurrentPosition()
        {
            return rectTransform.anchoredPosition;
        }
    }
}