using System.IO;
using System.Reflection;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    public static class ColorVisualAssetCreator
    {
        [MenuItem("Assets/Create Color Visual Assets From File", false, 100)]
        private static void CreateFromFile()
        {
            var selection = Selection.activeObject;

            if (selection is not TextAsset textAsset)
            {
                return;
            }

            var colors = textAsset.text.Split(",");

            string filePath = AssetDatabase.GetAssetPath(textAsset);
            string directory = Path.GetDirectoryName(filePath);

            foreach (string colorString in colors)
            {
                string trimmed = colorString.Trim();
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    continue;
                }

                if (ColorUtility.TryParseHtmlString(trimmed, out var color))
                {
                    CreateColorVisualAsset(directory, trimmed, color);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private static void CreateColorVisualAsset(string directory, string rawName, Color color)
        {
            // Create instance of your ScriptableObject
            ColorVisualAsset asset = ScriptableObject.CreateInstance<ColorVisualAsset>();

            // Force assign color property if it has private setter
            SetPrivateProperty(asset, nameof(asset.Color), color);

            // Also set ID or name if needed
            string fixedName = rawName.Replace(" ", "_").Replace("#", "");
            SetPrivateField(asset, "id", fixedName);

            string assetPath = Path.Combine(directory, fixedName + "_ColorVisual.asset");
            AssetDatabase.CreateAsset(asset, assetPath);

            Debug.Log($"Created ColorVisualAsset: {fixedName} ({color})");
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