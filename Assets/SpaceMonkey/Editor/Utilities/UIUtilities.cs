using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Editor.Utilities
{
    public static class UIUtilities
    {
        [MenuItem("Space Monkey/UI/Anchors to Corners %#w")]
        private static void AnchorsToCorners()
        {
            foreach (Transform transform in Selection.transforms)
            {
                RectTransform t = transform as RectTransform;
                RectTransform pt = Selection.activeTransform.parent as RectTransform;

                if (t == null || pt == null) return;

                Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
                Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

                t.anchorMin = newAnchorsMin;
                t.anchorMax = newAnchorsMax;
                t.offsetMin = t.offsetMax = new Vector2(0, 0);
            }
        }

        [MenuItem("Space Monkey/Debug/Print Selected Object")]
        private static void WorldPosition()
        {
            GameObject obj = Selection.activeGameObject;
            string str =
                $"Name: {obj.name} - Local Position: {obj.transform.localPosition} - Position: {obj.transform.position}";
            if (obj.TryGetComponent(out RectTransform rt))
            {
                str += $" - {rt.rect.width}x{rt.rect.height}";
            }

            Debug.Log(str);
        }
    }
}