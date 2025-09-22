using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardDecline;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard
{
    public class CreditCardViewController: BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly AccountService _accountService;

        public CreditCardViewController(PresenterService presenterService,NavigationPresenterService navigationPresenterService, AccountService accountService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _accountService = accountService;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }

        internal void OnNext()
        {
            if (_accountService.Model.Account.Week >= 3)
            {
                PresenterService.Show<CreditCardSplashView>().Forget();   
            }
            else
            {
                PresenterService.Show<CreditCardDeclineView>().Forget();   
            }
        }
    }
}