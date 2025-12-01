using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using UIService.Runtime.Presenter;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep1 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 1;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }
        
        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<CategorySelectionView>();
            var categorySelectionView = _presenterService.GetPresenter<CategorySelectionView>();
            _tutorialService.Value.ShowArrow(categorySelectionView.LabelText)
               .ShowMask(categorySelectionView.TutorialCategory.rectTransform);
            await _tutorialService.Value.WaitForObservable(categorySelectionView.CategorySelectedObservable);
        }

        public void Hide()
        {
        }
    }
}