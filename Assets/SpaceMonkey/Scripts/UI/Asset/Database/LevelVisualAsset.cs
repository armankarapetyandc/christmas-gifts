using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "LevelVisualAsset", menuName = "Space Monkey/Database/Level Visual Asset", order = 0)]
    public class LevelVisualAsset : VisualAsset
    {
        [field: SerializeField] public Sprite LevelIconSprite { get; private set; }
        [field: SerializeField] public Sprite BrokenLevelIconSprite { get; private set; }
        [field: SerializeField] public Color BackgroundColor { get; private set; }
    }
}