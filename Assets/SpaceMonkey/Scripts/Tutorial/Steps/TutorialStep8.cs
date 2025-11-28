using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep8 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 8;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }

        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<BusinessSetupCelebrationView>();
            var businessSetupCelebrationView = _presenterService.GetPresenter<BusinessSetupCelebrationView>();
            var nextButton = businessSetupCelebrationView.NextButton;
            var nextRect = (RectTransform) nextButton.transform;
            _tutorialService.Value.ShowArrow(nextRect).ShowMask(nextRect);
            await _tutorialService.Value.WaitForObservable(nextButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}