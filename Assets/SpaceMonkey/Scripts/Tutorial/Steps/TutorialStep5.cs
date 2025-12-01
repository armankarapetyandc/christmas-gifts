using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep5 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 5;

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
            var hashtagVerticalList = businessPreviewView.HashtagVerticalList;
            var hashtagVerticalListRect = (RectTransform) hashtagVerticalList.transform;
            _tutorialService.Value.ShowArrow(hashtagVerticalListRect).ShowMask(hashtagVerticalListRect)
                .ShowFunnySlideOut(FunnySlideOutTexts.Step51)
                .AddArrowYOffset(300);
            await _tutorialService.Value.WaitForObservable(hashtagVerticalList.SelectMore);
            _tutorialService.Value.HideTutorialView();
        }

        public void Hide()
        {
        }
    }
}