using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup
{
    public class BusinessSetupController : BasePresenterController
    {
        public BusinessSetupController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OnBack()
        {
            PresenterService.HidePreviousAndShow<CategorySelectionView>().Forget();
        }
    }
}