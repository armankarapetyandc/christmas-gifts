using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Competition;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Map;
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
        private PopupPresenterService _popupPresenterService;
        private readonly MapConfig _mapConfig;

        public ProfitController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, VisualAssetDatabase visualAssetDatabase,
            AccountService accountService,WeekSimulationContext weekSimulationContext,
            PopupPresenterService popupPresenterService, MapConfig mapConfig) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
            _mapConfig = mapConfig;
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

        internal float TotalCash()
        {
            return _weekSimulationContext.WeekSimulation.Money.CurrentValue;
        }

        internal async UniTaskVoid OnNext(bool isWeekEnd)
        {
            _weekSimulationContext.WeekSimulation.GrantReward();
            var newPlace = HasNewAppearedPlaces();
            if (newPlace != null)
            {
                await _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
                {
                    Type = MainNavigationType.Map,
                    IsWeekEnd = isWeekEnd
                });
                await UniTask.WaitUntil(() => PresenterService.GetPresenter<MapView>() != null);
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

                var mapView = PresenterService.GetPresenter<MapView>();
                mapView.FocusOnPlace(newPlace.Id);
                return;
            }
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub,
                IsWeekEnd = isWeekEnd
            }).Forget();
        }

        public void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }
        
        private IMapPlace HasNewAppearedPlaces()
        {
            var currentWeek = _accountService.Model.Account.Week;
            var appearedPlaces = _accountService.Model.Account.AppearedPlaces;
            var placesToAppear = _mapConfig.GetAppearedMapPlaces(_mapConfig.Places, currentWeek);
            return placesToAppear.FirstOrDefault(place => appearedPlaces.Contains(place.Id) == false);
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
                                                                           pair.Key.MaterialPackagingPrice!.Value) *
                pair.Value + pair.Key.ShippingCost!.Value);
        }
    }
}