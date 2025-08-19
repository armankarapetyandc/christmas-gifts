using System;
using System.Collections.Generic;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityViewController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        
        public ProductionCapacityViewController(PresenterService presenterService, 
            VisualAssetDatabase visualAssetDatabase, GameConfig gameConfig) : base(presenterService)
        {
            _visualAssetDatabase = visualAssetDatabase;
            _gameConfig = gameConfig;
        }

        internal ProductionLevelInfo[] RetrieveInfo()
        {
            return _gameConfig.ProductionLevels;
        }
        
        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void OnBack()
        {
            Debug.LogError("Clicked on the back button");
            //PresenterService.Show<>().Forget();
        }
    }
}