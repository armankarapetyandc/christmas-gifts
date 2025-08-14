using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    [CustomEditor(typeof(SpriteVisualAsset))]
    public class SpriteVisualAssetEditor : UnityEditor.Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            SpriteVisualAsset asset = (SpriteVisualAsset)target;

            if (asset.Sprite?.texture == null)
                return null;

            return CreatePreviewFromSprite(asset.Sprite, width, height);
        }

        private Texture2D CreatePreviewFromSprite(Sprite sprite, int width, int height)
        {
            // Create temporary render texture
            RenderTexture tempRT = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            RenderTexture previousRT = RenderTexture.active;
            RenderTexture.active = tempRT;

            // Clear to transparent
            GL.Clear(true, true, Color.clear);

            // Calculate UV coordinates for the sprite within its texture
            Texture2D sourceTexture = sprite.texture;
            Rect spriteRect = sprite.rect;

            Vector2 uvMin = new Vector2(spriteRect.x / sourceTexture.width, spriteRect.y / sourceTexture.height);
            Vector2 uvMax = new Vector2((spriteRect.x + spriteRect.width) / sourceTexture.width,
                (spriteRect.y + spriteRect.height) / sourceTexture.height);

            // Draw the sprite portion to the render texture
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, width, height, 0);

            Graphics.DrawTexture(
                new Rect(0, 0, width, height),
                sourceTexture,
                new Rect(uvMin.x, uvMin.y, uvMax.x - uvMin.x, uvMax.y - uvMin.y),
                0, 0, 0, 0
            );

            GL.PopMatrix();

            // Read back to Texture2D
            Texture2D preview = new Texture2D(width, height, TextureFormat.RGBA32, false);
            preview.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            preview.Apply();

            // Cleanup
            RenderTexture.active = previousRT;
            RenderTexture.ReleaseTemporary(tempRT);

            return preview;
        }
    }
}