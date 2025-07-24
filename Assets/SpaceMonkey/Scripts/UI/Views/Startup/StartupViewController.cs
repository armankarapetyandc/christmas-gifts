using R3;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Startup
{
    public class StartupViewController : BasePresenterController
    {
        private readonly ReactiveProperty<bool> _uiInteractableProperty = new ReactiveProperty<bool>(true);

        public ReadOnlyReactiveProperty<bool> UIInteractable => _uiInteractableProperty.ToReadOnlyReactiveProperty();

        public StartupViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void StartNewBusiness()
        {
            _uiInteractableProperty.Value = false;
        }

        public void LoadCurrentBusiness()
        {
            _uiInteractableProperty.Value = false;
        }
    }
}