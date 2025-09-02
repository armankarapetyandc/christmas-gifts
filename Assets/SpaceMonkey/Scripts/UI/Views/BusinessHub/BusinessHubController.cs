using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Marketing;
using SpaceMonkey.Scripts.UI.Views.Orders;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using SpaceMonkey.Scripts.UI.Views.Staff;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly WeekSimulationContext _weekSimulationContext;

        public BusinessHubController(PresenterService presenterService,
            AccountService accountService, VisualAssetDatabase visualAssetDatabase,
            NavigationPresenterService navigationPresenterService,
            WeekSimulationContext weekSimulationContext) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
            _navigationPresenterService = navigationPresenterService;
            _weekSimulationContext = weekSimulationContext;
        }


        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }


        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal void ShowProductView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }


        internal void StartWeek()
        {
            _weekSimulationContext.Run();
        }

        internal void ShowProductionView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductionCapacityView>().Forget();
        }

        public void ShowMarketingView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<MarketingView>().Forget();
        }
        
        public void ShowStaffView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<StaffView>().Forget();
        }
    }
}