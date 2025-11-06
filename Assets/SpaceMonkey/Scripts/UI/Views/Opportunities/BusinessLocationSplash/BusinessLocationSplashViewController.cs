using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash
{
    public class BusinessLocationSplashViewController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public BusinessLocationSplashViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        public void OnNext()
        {
            PresenterService.Hide();
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Map
            }).Forget();
        }
    }
}