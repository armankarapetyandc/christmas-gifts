using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep7 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 7;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }

        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<BusinessPreviewView>();
            var businessPreviewView = _presenterService.GetPresenter<BusinessPreviewView>();
            var saveButton = businessPreviewView.SaveButton;
            var saveRect = (RectTransform) saveButton.transform;
            _tutorialService.Value.ShowArrow(saveRect).ShowMask(saveRect).ShowFunnySlideOut(FunnySlideOutTexts.Step7);
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}