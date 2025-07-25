using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Startup;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Splash
{
    public class SplashViewController : BasePresenterController
    {
        public SplashViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        public async UniTaskVoid OpenNextView()
        {
            await UniTask.Delay(2000);
            await PresenterService.Hide();
            PresenterService.Show<StartupView>().Forget();
        }
    }
}