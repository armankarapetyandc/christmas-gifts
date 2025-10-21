using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsuranceCongratulation
{
    public class DisasterInsuranceCongratulationViewController : BasePresenterController
    {
        public DisasterInsuranceCongratulationViewController(PresenterService presenterService) : base(presenterService)
        {
            
        }
        
        internal void OnNext()
        {
          //  PresenterService.Show<BigOrderDetailsView>();
        }
    }
}