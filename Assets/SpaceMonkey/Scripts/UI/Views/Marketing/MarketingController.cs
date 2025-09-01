using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly WeekSimulationContext _weekSimulationContext;
        private readonly AccountService _accountService;
        
        [Inject] private GameConfig _gameConfig;

        public MarketingController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, WeekSimulationContext weekSimulationContext,
            AccountService accountService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _weekSimulationContext = weekSimulationContext;
            _accountService = accountService;
        }

        private void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        internal void UpdateMarketingFeatures(List<MarketingFeature> marketingFeatures)
        {
            _accountService.Model.Account.UpdateMarketingFeatures(marketingFeatures);
            _accountService.SaveAsync().Forget();
            OnBack();
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal MarketingInfo[] GetMarketingInfos()
        {
            return _gameConfig.MarketingInfos;
        }
    }
}