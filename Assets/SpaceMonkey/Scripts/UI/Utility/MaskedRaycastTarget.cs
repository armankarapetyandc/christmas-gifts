using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility
{
    public class MaskedRaycastTarget : Image
    {
        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint);

            // Convert local point to normalized 0-1 UV
            Vector2 uv = Rect.PointToNormalized(rectTransform.rect, localPoint);

            // Sample the texture alpha
            if (sprite != null)
            {
                Color color = sprite.texture.GetPixelBilinear(uv.x, uv.y);
                return color.a > 0.1f; // Only clickable if visible
            }

            return base.Raycast(sp, eventCamera);
        }
    }
}