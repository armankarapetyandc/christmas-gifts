using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BigOrder;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.DeleteProduct;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderViewController : BasePresenterController

    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly PopupPresenterService _popupPresenterService;
        private AccountService _accountService;

        private List<WeekSimulationV2.OrderEntry> _orderEntries;

        public BigOrderViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, VisualAssetDatabase visualAssetDatabase,
            PopupPresenterService popupPresenterService, AccountService accountService) : base(presenterService)
        {
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            _visualAssetDatabase = visualAssetDatabase;
            _popupPresenterService = popupPresenterService;
        }

        internal void OnBack(MainNavigationType type)
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = type
            }).Forget();
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }
        
        internal List<WeekSimulationV2.OrderEntry> GetOrders()
        {
            _orderEntries = _accountService.Model.Account.GetOrders();
            return _orderEntries;
        }

        public void OnDecline(MainNavigationType type)
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = type
            }).Forget();
        }

        public async UniTaskVoid OnAccept(MainNavigationType type)
        {
            _accountService.Model.Account.CreateBigOrder();
            _accountService.Model.Account.BigOrderGameData.OrderEntries = _orderEntries;
            await _accountService.SaveAsync();
            PresenterService.Show<BigOrderCongratulationView>(new BigOrderCongratulationView.Data()
            {
                Type = type
            });
        }
    }
}