using UnityEngine;

namespace ChristmasGifts.Scripts
{
    public static class Utils
    {
        public static T FindComponentInParents<T>(Transform startTransform) where T : class
        {
            Transform currentTransform = startTransform;
            while (currentTransform != null)
            {
                // Get all components on the current transform
                T component = currentTransform.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }

                // Move up the hierarchy
                currentTransform = currentTransform.parent;
            }

            return null; // No component implementing T found in the hierarchy
        }
    }
}