using ContextLoaderService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Startup
{
    public class StartupViewController : BasePresenterController
    {
        internal readonly AccountService AccountService;
        private readonly LoadingService _loadingService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly ReactiveProperty<bool> _uiInteractableProperty = new ReactiveProperty<bool>(true);

        public ReadOnlyReactiveProperty<bool> UIInteractable => _uiInteractableProperty.ToReadOnlyReactiveProperty();

        public StartupViewController(PresenterService presenterService, AccountService accountService,
            LoadingService loadingService, NavigationPresenterService navigationPresenterService) : base(
            presenterService)
        {
            AccountService = accountService;
            _loadingService = loadingService;
            _navigationPresenterService = navigationPresenterService;
        }

        public void StartNewBusiness()
        {
            _uiInteractableProperty.Value = false;
            AccountService.CreateNewAccount();
            PresenterService.HidePreviousAndShow<CategorySelectionView>().Forget();
        }

        public async UniTaskVoid LoadCurrentBusiness()
        {
            _uiInteractableProperty.Value = false;
            await _loadingService.BeginLoading(AccountService.LoadAsync().ToLoadingUnit(),
                _navigationPresenterService.Show<MainNavigation>().ToLoadingUnit());
            _uiInteractableProperty.Value = true;
        }
    }
}