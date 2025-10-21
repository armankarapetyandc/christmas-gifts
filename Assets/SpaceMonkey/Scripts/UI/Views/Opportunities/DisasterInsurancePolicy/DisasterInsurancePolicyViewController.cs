using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsurancePolicy
{
    public class DisasterInsurancePolicyViewController : BasePresenterController
    {
        public DisasterInsurancePolicyViewController(PresenterService presenterService) : base(presenterService)
        {
        }
        
        public void OnCancel()
        {
           // PresenterService.Show<BigOrderCanceledView>().Forget();
        }
    }
}