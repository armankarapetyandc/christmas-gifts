using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard
{
    public class CreditCardViewController: BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public CreditCardViewController(PresenterService presenterService,NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
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
            PresenterService.Show<CreditCardSplashView>().Forget();
        }
    }
}