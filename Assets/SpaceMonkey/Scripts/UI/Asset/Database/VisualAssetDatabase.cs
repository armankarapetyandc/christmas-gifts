using Services.AssetDatabaseService;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [CreateAssetMenu(fileName = "VisualAssetDatabase", menuName = "Space Monkey/Database/Visual Asset Database",
        order = 0)]
    public class VisualAssetDatabase : ScriptablesDataBase<VisualAsset>
    {
        public TAsset GetResourceForAsset<TAsset>(string id) where TAsset : VisualAsset
        {
            var resource = GetResource(id);
            return resource != null && resource is TAsset asset ? asset : null;
        }
    }
}