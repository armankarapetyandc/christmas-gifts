using System;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.Investment;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Map;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign
{
    public class BusinessLocationSignViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly GameConfig _gameConfig;
        private readonly InvestmentSimulator _investmentSimulator;
        private readonly NavigationPresenterService _navigationPresenterService;
        public float CashAmount => _accountService.Model.Account.Money;
        public int LocationPrice => _gameConfig.NewLocationInfo.Price;
        
        public BusinessLocationSignViewController(PresenterService presenterService, 
            AccountService accountService, GameConfig gameConfig, 
            InvestmentSimulator investmentSimulator, NavigationPresenterService navigationPresenterService)
            : base(presenterService)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
            _investmentSimulator = investmentSimulator;
            _navigationPresenterService = navigationPresenterService;
        }

        public void OnSign(int dataPlaceId)
        {
            var result = _investmentSimulator.BuyPlace(dataPlaceId);
            if (result)
            {
                PresenterService.Show<BusinessLocationSplashView>(new BusinessLocationSplashView.Data
                {
                    PlaceId = dataPlaceId
                }).Forget();
            }
        }
        
        public async UniTaskVoid OnBack(int dataPlaceId)
        {
            await PresenterService.Hide();
            await _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Map
            });
            
            var mapView = PresenterService.GetPresenter<MapView>();

            await UniTask.WaitWhile(() =>
            {
                mapView = PresenterService.GetPresenter<MapView>();
                return mapView == null;
            });

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            mapView.FocusOnPlace(dataPlaceId);
        }
    }
}