using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.BigOrder
{
    public class BigOrderPopupController : BasePresenterController
    {
        public BigOrderPopupController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OpenBigOrderView()
        {
            PresenterService.Show<BigOrderView>().Forget();
        }
    }
}