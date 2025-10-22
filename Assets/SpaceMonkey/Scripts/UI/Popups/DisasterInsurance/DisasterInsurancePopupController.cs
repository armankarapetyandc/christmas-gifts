using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.DisasterInsurance;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.DisasterInsurance
{
    public class DisasterInsurancePopupController : BasePresenterController
    {
        public DisasterInsurancePopupController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void ConfirmClicked()
        {
            PresenterService.Show<DisasterInsuranceView>().Forget();
        }
    }
}