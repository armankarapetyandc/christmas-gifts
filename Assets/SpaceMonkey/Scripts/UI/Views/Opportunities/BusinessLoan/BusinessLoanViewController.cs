using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BusinessLoanInfo;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanSplash;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoan
{
    public class BusinessLoanViewController: BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly NavigationPresenterService _navigationPresenterService;

        public BusinessLoanViewController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
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
            PresenterService.Show<BusinessLoanSplashView>().Forget();   
        }

        public void OnInfo()
        {
            _popupPresenterService.Show<BusinessLoanInfoPopup>().Forget();
        }
    }
}