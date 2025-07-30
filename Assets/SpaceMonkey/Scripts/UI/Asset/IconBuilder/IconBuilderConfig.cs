using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceMonkey.Scripts.UI.Asset.IconBuilder
{
    [CreateAssetMenu(fileName = "IconBuilder", menuName = "Space Monkey/Icon Builder Config")]
    public class IconBuilderConfig : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> ShapesSprites { get; private set; }
        [field: SerializeField] public List<Sprite> IconSprites { get; private set; }
        [field: SerializeField] public List<Color> BackgroundColors { get; private set; }

        public Sprite GetShapeSprite(string shapeName)
        {
            foreach (var sprite in ShapesSprites)
            {
                if (sprite != null && sprite.name == shapeName)
                {
                    return sprite;
                }
            }

            Debug.LogError($"Shape sprite with name '{shapeName}' not found.");
            return null;
        }

        public Sprite GetIconSprite(string iconName)
        {
            foreach (var sprite in IconSprites)
            {
                if (sprite != null && sprite.name == iconName)
                {
                    return sprite;
                }
            }

            Debug.LogError($"Icon sprite with name '{iconName}' not found.");
            return null;
        }
    }
}