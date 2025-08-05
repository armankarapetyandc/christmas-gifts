using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Navigation.Bottom
{
    public class MainNavigationController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public MainNavigationController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        public UniTask ShowPresenter<T>(IPresenterData data = null) where T : BasePresenter
        {
            return PresenterService.Show<T>(data);
        }

        public UniTask HidePreviousAndShow<T>(IPresenterData data = null) where T : BasePresenter
        {
            return PresenterService.HidePreviousAndShow<T>(data);
        }

        public void HideMainNavigation()
        {
            _navigationPresenterService.Hide<MainNavigation>();
        }
    }
}