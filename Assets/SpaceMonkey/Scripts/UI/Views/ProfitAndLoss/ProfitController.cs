using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ProfitController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly AccountService _accountService;
        private readonly WeekSimulationContext _weekSimulationContext;

        public ProfitController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, VisualAssetDatabase visualAssetDatabase,
            AccountService accountService,WeekSimulationContext weekSimulationContext) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _visualAssetDatabase = visualAssetDatabase;
            _accountService = accountService;
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

        internal void OnNext()
        {
            _weekSimulationContext.WeekSimulation.GrantReward();
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        public void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }
        
        internal Dictionary<Profile.Product, int> GetTotalQuantitiesByProduct(bool useSimulation)
        {
            if (useSimulation)
            {
                return _weekSimulationContext.WeekSimulation.CurrentWeek!.Value.GetTotalQuantitiesByProduct();
            }
            return _accountService.Model.Account.WeeksV2[^1].GetTotalQuantitiesByProduct();
        }
        internal float GetWeekProfit(bool useSimulation)
        {
            return GetTotalQuantitiesByProduct(useSimulation).Sum(pair => pair.Key.Profit!.Value * pair.Value);
        }
        internal float GetWeekRevenue(bool useSimulation)
        {
            return GetTotalQuantitiesByProduct(useSimulation).Sum(pair => pair.Key.ProductPrice!.Value * pair.Value);
        }
        internal float GetWeekTotalExpenses(bool useSimulation)
        {
            return GetTotalQuantitiesByProduct(useSimulation).Sum(pair => (pair.Key.MaterialPrice!.Value +
                                                                           pair.Key.MaterialPackagingPrice!.Value +
                                                                           pair.Key.ShippingCost!.Value) * pair.Value);
        }
    }
}