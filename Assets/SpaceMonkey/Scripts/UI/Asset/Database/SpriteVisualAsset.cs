using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "SpriteVisualAsset", menuName = "Space Monkey/Database/Sprite Visual Asset", order = 0)]
    public class SpriteVisualAsset : VisualAsset
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}