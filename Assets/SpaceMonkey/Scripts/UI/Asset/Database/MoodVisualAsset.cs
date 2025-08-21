using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "MoodVisualAsset", menuName = "Space Monkey/Database/Mood Visual Asset", order = 0)]
    public class MoodVisualAsset : SpriteVisualAsset
    {
        [field: SerializeField]
        [Range(1, 100)]
        public float MoodValue { get; private set; }
    }
}