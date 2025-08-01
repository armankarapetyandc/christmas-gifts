using Services.AssetDatabaseService;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset
{
    [CreateAssetMenu(fileName = "VisualAsset", menuName = "Space Monkey/Database/Visual Asset", order = 0)]
    public class VisualAsset : ScriptableAsset
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}