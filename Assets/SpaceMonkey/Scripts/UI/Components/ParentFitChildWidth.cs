using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    [ExecuteAlways]
    public class ParentFitChildWidth : MonoBehaviour
    {
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private RectTransform parent;
        [SerializeField] private RectTransform child;
        [SerializeField] private float paddingLeft = 0f;
        [SerializeField] private float paddingRight = 0f;
        [SerializeField] private float paddingTop = 0f;
        [SerializeField] private float paddingBottom = 0f;


#if UNITY_EDITOR
        private void OnValidate()
        {
            parent ??= GetComponent<RectTransform>();
            layoutElement ??= GetComponent<LayoutElement>();
        }
#endif

        private void OnRectTransformDimensionsChange()
        {
            if (child == null) return;
            float newWidth = child.rect.width + paddingLeft + paddingRight;
            float newHeight = child.rect.height + paddingTop + paddingBottom;

            if (layoutElement != null)
            {
                // If LayoutElement exists, update preferred sizes
                layoutElement.preferredWidth = newWidth;
                layoutElement.preferredHeight = newHeight;
            }
            else
            {
                // Otherwise, resize RectTransform directly
                parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
                parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            }
        }
    }
}