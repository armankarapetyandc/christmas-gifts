using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<TAsset> GetResourcesForAsset<TAsset>(Predicate<TAsset> predicate)
        {
            return predicate != null
                ? Assets.Where(asset => asset is TAsset).Cast<TAsset>().Where(asset => predicate(asset))
                : Assets.Where(asset => asset is TAsset).Cast<TAsset>();
        }
    }
}