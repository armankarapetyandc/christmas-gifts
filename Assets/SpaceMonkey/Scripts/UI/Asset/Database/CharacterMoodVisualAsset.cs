using SpaceMonkey.Scripts.Utilities;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    public class CharacterMoodVisualAsset : SpriteVisualAsset
    {
        [field: SerializeField] public RangeValue MoodRange { get; private set; }
    }
}