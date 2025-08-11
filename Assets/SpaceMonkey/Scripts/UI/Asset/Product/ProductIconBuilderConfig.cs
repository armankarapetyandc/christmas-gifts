using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Product
{
    [CreateAssetMenu(fileName = "ProductIconBuilder", menuName = "Space Monkey/Product Icon Builder Config")]
    public class ProductIconBuilderConfig : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> IconSprites { get; private set; }
        [field: SerializeField] public List<Color> BackgroundColors { get; private set; }
        
        public Sprite GetIconSprite(string iconName)
        {
            var nameSprite = iconName.Split(' ').First();
            foreach (var sprite in IconSprites)
            {
                if (sprite != null && sprite.name == nameSprite)
                {
                    return sprite;
                }
            }

            Debug.LogError($"Icon sprite with name '{iconName}' not found.");
            return null;
        }
    }
}