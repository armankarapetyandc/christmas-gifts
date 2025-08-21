using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrdersViewController:BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly WeekSimulationContext _weekSimulationContext;

        public OrdersViewController(PresenterService presenterService,AccountService accountService,VisualAssetDatabase visualAssetDatabase,WeekSimulationContext weekSimulationContext) : base(presenterService)
        {
            _accountService = accountService;
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
        public List<Customer> GetSimulationCustomers()
        {
            return _weekSimulationContext.WeekSimulation.Customers;
        }
    }
}