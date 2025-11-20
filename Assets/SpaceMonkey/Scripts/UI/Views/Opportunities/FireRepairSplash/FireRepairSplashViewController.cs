using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.FireRepairSplash
{
    public class FireRepairSplashViewController : BasePresenterController
    {

        public FireRepairSplashViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        internal void OnNext()
        {
            PresenterService.HidePreviousAndShow<ProductionCapacityView>(new ProductionCapacityView.Data()).Forget();
        }
    }
}
