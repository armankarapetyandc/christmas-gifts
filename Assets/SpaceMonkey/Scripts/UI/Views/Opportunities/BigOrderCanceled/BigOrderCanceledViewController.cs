using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCanceled
{
    public class BigOrderCanceledViewController : BasePresenterController
    {
        private NavigationPresenterService _navigationPresenterService;

        public BigOrderCanceledViewController(NavigationPresenterService navigationPresenterService,
            PresenterService presenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }


        public void OnOkClicked()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }
    }
}