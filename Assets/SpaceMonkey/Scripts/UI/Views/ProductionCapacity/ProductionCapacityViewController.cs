using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment;
using SpaceMonkey.Scripts.UI.Views.LevelUpdate;
using SpaceMonkey.Scripts.UI.Views.Opportunities.FireRepairSplash;
using TMPro;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityViewController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService  _popupPresenterService;
        private readonly FireSimulator _fireSimulator;

        public ProductionCapacityViewController(PresenterService presenterService,
            VisualAssetDatabase visualAssetDatabase, GameConfig gameConfig, AccountService accountService,
            NavigationPresenterService navigationPresenterService, PopupPresenterService popupPresenterService,
            FireSimulator fireSimulator) : base(presenterService)
        {
            _visualAssetDatabase = visualAssetDatabase;
            _gameConfig = gameConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
            _fireSimulator = fireSimulator;
        }

        internal async UniTask<LevelProdCap> OpenUpgradeEquipmentPopup(LevelProdCap level)
        {
            var tcs = new UniTaskCompletionSource<LevelProdCap>();
            _popupPresenterService.Show<UpgradeEquipmentPopup>(new UpgradeEquipmentPopup.Data
            {
                UpgradeLevelProdCap = level,
                Result = tcs
            });
            return await tcs.Task;;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        public void UpgradeLevel(int levelNumber, Sprite levelSprite, TMP_FontAsset levelFontAsset)
        {
            PresenterService.Show<LevelUpdateView>(new LevelUpdateView.Data
            {
                LevelNumber = levelNumber,
                LevelSprite = levelSprite,
                IsUpgraded = true,
                FontAsset = levelFontAsset
            });
        }
        
        public float GetFireRepairCost()
        {
            return _fireSimulator.GetRepairCost();
        }
        
        public UniTask<bool> RepairFireDamage(float cost)
        {
            return _fireSimulator.RepairFire(cost);
        }
        
        public void ShowFireRepairSplash(LevelProdCap level)
        {
            PresenterService.Show<FireRepairSplashView>(new FireRepairSplashView.Data
            {
                Level = level
            }).Forget();
        }
    }
}