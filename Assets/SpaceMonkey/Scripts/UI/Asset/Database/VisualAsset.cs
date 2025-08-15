using Services.AssetDatabaseService;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    public abstract class VisualAsset : ScriptableAsset
    {
        [field: SerializeField] public VisualAssetType Type { get; private set; }
    }
}