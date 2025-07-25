using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Splash
{
    public class SplashViewController : BasePresenterController
    {
        public SplashViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OpenNextView()
        {
            Debug.LogError("Open next view");
        }
    }
}