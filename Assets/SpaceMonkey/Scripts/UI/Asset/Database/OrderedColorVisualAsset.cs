using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "OrderedColorVisualAsset",
        menuName = "Space Monkey/Database/Ordered Color Visual Asset", order = 0)]
    public class OrderedColorVisualAsset : ColorVisualAsset
    {
        [field: SerializeField] public int Order { get; private set; }
    }
}