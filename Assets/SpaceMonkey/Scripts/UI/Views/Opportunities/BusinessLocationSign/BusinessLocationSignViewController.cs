using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign
{
    public class BusinessLocationSignViewController : BasePresenterController
    {
        public BusinessLocationSignViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OnSign()
        {
            PresenterService.Show<BusinessLocationSplashView>().Forget();
        }
        
        public void OnBack()
        {
            PresenterService.HidePreviousAndShow<OpportunitiesView>().Forget();
        }
    }
}