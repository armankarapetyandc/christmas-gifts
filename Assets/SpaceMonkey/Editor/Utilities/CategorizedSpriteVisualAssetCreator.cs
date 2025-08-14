using System;
using System.IO;
using System.Reflection;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    public static class CategorizedSpriteVisualAssetCreator
    {
        [MenuItem("Assets/Create Categorized Sprite Visual Assets From Selection", false, 100)]
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

            CategorizedSpriteVisualAsset asset = ScriptableObject.CreateInstance<CategorizedSpriteVisualAsset>();

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
            var type = target.GetType();
            PropertyInfo property = null;

            // Search up the inheritance chain
            while (type != null)
            {
                property = type.GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (property != null)
                    break;
                type = type.BaseType;
            }

            if (property != null)
            {
                try
                {
                    property.SetValue(target, value, null);
                }
                catch (ArgumentException)
                {
                    // Property has no setter, try to find and set the backing field
                    string backingFieldName = $"<{propertyName}>k__BackingField";
                    if (!TrySetBackingField(target, backingFieldName, value))
                    {
                        // Try common backing field patterns
                        string lowerFieldName = char.ToLower(propertyName[0]) + propertyName.Substring(1);
                        if (!TrySetBackingField(target, lowerFieldName, value))
                        {
                            string underscoreFieldName = "_" + lowerFieldName;
                            if (!TrySetBackingField(target, underscoreFieldName, value))
                            {
                                Debug.LogWarning(
                                    $"Property '{propertyName}' has no setter and backing field not found on {target.GetType()}");
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Property '{propertyName}' not found on {target.GetType()} or its base types");
            }
        }

        private static bool TrySetBackingField(object target, string fieldName, object value)
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
                return true;
            }

            return false;
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