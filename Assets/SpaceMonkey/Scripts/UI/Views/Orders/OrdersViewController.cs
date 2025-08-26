using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.ProductionAlert;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrdersViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly PopupPresenterService _popupPresenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly WeekSimulationContext _weekSimulationContext;

        public OrdersViewController(PresenterService presenterService, AccountService accountService,
            PopupPresenterService popupPresenterService,
            VisualAssetDatabase visualAssetDatabase,
            WeekSimulationContext weekSimulationContext) : base(presenterService)
        {
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
            _visualAssetDatabase = visualAssetDatabase;
            _weekSimulationContext = weekSimulationContext;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal List<Customer> GetSimulationCustomers()
        {
            return _weekSimulationContext.WeekSimulation.Customers;
        }

        internal Observable<int> GetAvailableProdCapObservable()
        {
            return _weekSimulationContext.WeekSimulation.AvailableProdCap;
        }
        
        internal Observable<float> GetMoneyObservable()
        {
            return _weekSimulationContext.WeekSimulation.Money;
        }

        internal async UniTask<bool> TryShipOrder(Customer customer)
        {
            if (!_weekSimulationContext.WeekSimulation.TryShipOrder(customer))
            {
                var data = new ProductionAlertPopup.Data();
                _popupPresenterService.Show<ProductionAlertPopup>(data).Forget();
                await data.CompletionSource.Task;
                return false;
            }

            return true;
        }

        public void FinishWeek()
        {
            _weekSimulationContext.Finish();
        }
    }
}