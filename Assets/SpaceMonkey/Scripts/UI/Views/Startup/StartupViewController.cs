using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Startup
{
    public class StartupViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly ReactiveProperty<bool> _uiInteractableProperty = new ReactiveProperty<bool>(true);

        public ReadOnlyReactiveProperty<bool> UIInteractable => _uiInteractableProperty.ToReadOnlyReactiveProperty();

        public StartupViewController(PresenterService presenterService, AccountService accountService) : base(
            presenterService)
        {
            _accountService = accountService;
        }

        public void StartNewBusiness()
        {
            _uiInteractableProperty.Value = false;
            _accountService.CreateNewAccount();
            PresenterService.HidePreviousAndShow<CategorySelectionView>().Forget();
        }

        public void LoadCurrentBusiness()
        {
            // _uiInteractableProperty.Value = false;
        }
    }
}