using System.IO;
using System.Reflection;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    public static class SpriteVisualAssetCreator
    {
        [MenuItem("Assets/Create Sprite Visual Assets From Selection", false, 100)]
        private static void CreateFromSelectedSprites()
        {
            var selectedObjects = Selection.objects;

            foreach (var obj in selectedObjects)
            {
                // Try to get a sprite directly
                Sprite sprite = obj as Sprite;

                // If it's a texture, try loading all sprites in it
                if (sprite == null && obj is Texture2D texture)
                {
                    string path = AssetDatabase.GetAssetPath(texture);
                    var subAssets = AssetDatabase.LoadAllAssetsAtPath(path);
                    foreach (var sub in subAssets)
                    {
                        if (sub is Sprite s)
                            CreateVisualAsset(s);
                    }

                    continue;
                }

                // If it is a sprite, create asset directly
                if (sprite != null)
                    CreateVisualAsset(sprite);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateVisualAsset(Sprite sprite)
        {
            if (sprite == null) return;

            SpriteVisualAsset asset = ScriptableObject.CreateInstance<SpriteVisualAsset>();

            // Force set the Sprite property (private setter)
            SetPrivateProperty(asset, nameof(asset.Sprite), sprite);

            // Extract name and sanitize
            string fixedName = sprite.name.Replace(" ", "_");

            // Force set the private 'id' field (could be in a base class)
            SetPrivateField(asset, "id", fixedName);

            // Save asset next to sprite
            string spritePath = AssetDatabase.GetAssetPath(sprite);
            string dir = Path.GetDirectoryName(spritePath);
            string assetPath = Path.Combine(dir, sprite.name + "_VisualAsset.asset");

            AssetDatabase.CreateAsset(asset, assetPath);
            Debug.Log($"Created SpriteVisualAsset for: {sprite.name} (id={fixedName})");
        }

        private static void SetPrivateProperty(object target, string propertyName, object value)
        {
            var prop = target.GetType().GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(target, value, null);
            }
            else
            {
                Debug.LogWarning($"Property '{propertyName}' not found or not writable on {target.GetType()}");
            }
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var type = target.GetType();
            FieldInfo field = null;

            // Search up the inheritance chain
            while (type != null)
            {
                field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                    break;
                type = type.BaseType;
            }

            if (field != null)
            {
                field.SetValue(target, value);
            }
            else
            {
                Debug.LogWarning($"Field '{fieldName}' not found on {target.GetType()} or its base types");
            }
        }
    }
}