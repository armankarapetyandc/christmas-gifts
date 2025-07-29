using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup
{
    public class BusinessSetupController : BasePresenterController
    {
        public IconBuilderConfig IconBuilderConfig { get; }

        public BusinessSetupController(PresenterService presenterService, IconBuilderConfig iconBuilderConfig) : base(
            presenterService)
        {
            IconBuilderConfig = iconBuilderConfig;
        }
    }
}