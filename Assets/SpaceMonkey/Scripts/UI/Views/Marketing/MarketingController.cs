using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly WeekSimulationContext _weekSimulationContext;
        private readonly AccountService _accountService;

        public MarketingController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, WeekSimulationContext weekSimulationContext,
            AccountService accountService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _weekSimulationContext = weekSimulationContext;
            _accountService = accountService;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }
    }
}