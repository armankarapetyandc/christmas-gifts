using Cysharp.Threading.Tasks;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Splash
{
    public class SplashView : BasePresenterWithController<SplashViewController>
    {
        public override UniTask Initialize(IPresenterData data = null)
        {
            return UniTask.Delay(2000).ContinueWith(() =>
            {
                Controller.OpenNextView();
            });
        }
        
        

        public override void Dispose()
        {   
            
        }
    }
}