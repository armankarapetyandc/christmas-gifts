using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "CategorizedSpriteVisualAsset", menuName = "Space Monkey/Database/Categorized Sprite Visual Asset", order = 0)]
    public class CategorizedSpriteVisualAsset : SpriteVisualAsset
    {
        [field: SerializeField] public string Category { get; private set; }
    }
}