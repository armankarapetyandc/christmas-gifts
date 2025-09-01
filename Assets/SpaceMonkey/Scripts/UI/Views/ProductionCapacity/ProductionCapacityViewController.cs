using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityViewController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService  _popupPresenterService;

        public ProductionCapacityViewController(PresenterService presenterService,
            VisualAssetDatabase visualAssetDatabase, GameConfig gameConfig, AccountService accountService,
            NavigationPresenterService navigationPresenterService, PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _visualAssetDatabase = visualAssetDatabase;
            _gameConfig = gameConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
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
    }
}