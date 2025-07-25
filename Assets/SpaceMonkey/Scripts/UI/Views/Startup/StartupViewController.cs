using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
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

        public async UniTaskVoid StartNewBusiness()
        {
            _uiInteractableProperty.Value = false;
            await PresenterService.Hide();
            await PresenterService.Show<CategorySelectionView>(new CategorySelectionView.Data
            {
                BackHandler = async () =>
                {
                    await PresenterService.Hide();
                    await PresenterService.Show<StartupView>();
                }
            });
        }

        public void LoadCurrentBusiness()
        {
            _uiInteractableProperty.Value = false;
        }
    }
}