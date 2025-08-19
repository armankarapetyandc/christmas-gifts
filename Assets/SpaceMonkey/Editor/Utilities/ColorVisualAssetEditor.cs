using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    [CustomEditor(typeof(ColorVisualAsset),true)]
    [CanEditMultipleObjects]
    public class ColorVisualAssetEditor : UnityEditor.Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            ColorVisualAsset asset = (ColorVisualAsset)target;

            return CreateColorPreview(asset.Color, width, height);
        }

        private Texture2D CreateColorPreview(Color color, int width, int height)
        {
            Texture2D preview = new Texture2D(width, height, TextureFormat.RGBA32, false);

            // Fill entire texture with the color
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            preview.SetPixels(pixels);
            preview.Apply();

            return preview;
        }
    }
}