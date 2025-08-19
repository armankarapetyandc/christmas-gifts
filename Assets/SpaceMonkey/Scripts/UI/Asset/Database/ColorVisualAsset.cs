using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "ColorVisualAsset", menuName = "Space Monkey/Database/Color Visual Asset", order = 0)]
    public class ColorVisualAsset : VisualAsset
    {
        [field: SerializeField] public Color Color { get; private set; }
    }
}