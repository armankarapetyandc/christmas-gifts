using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Navigation.Bottom
{
    public class MainNavigationController : BasePresenterController
    {
        public MainNavigationController(PresenterService presenterService) : base(presenterService)
        {
        }

        public UniTask ShowPresenter<T>(IPresenterData data = null) where T : BasePresenter
        {
            return PresenterService.Show<T>(data);
        }

        public UniTask HidePreviousAndShow<T>(IPresenterData data = null) where T : BasePresenter
        {
            return PresenterService.HidePreviousAndShow<T>(data);
        }
    }
}