using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Startup;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Splash
{
    public class SplashViewController : BasePresenterController
    {
        public SplashViewController(PresenterService presenterService) : base(presenterService)
        {
        }
    }
}