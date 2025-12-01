using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep2 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 2;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }
        
        public async UniTask Show()
        {
            var categorySelectionView = _presenterService.GetPresenter<CategorySelectionView>();
            var nextButtonRect = (RectTransform)categorySelectionView.NextButton.transform;
            _tutorialService.Value.ShowArrow(nextButtonRect).ShowMask(nextButtonRect);
            await _tutorialService.Value.WaitForButtonPress(categorySelectionView.NextButton);
        }

        public void Hide()
        {
        }
    }
}