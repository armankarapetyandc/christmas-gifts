using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep3 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 3;

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
            var inputFiled = (RectTransform)businessPreviewView.NameInputField.transform;
            _tutorialService.Value.ShowArrow(inputFiled).ShowMask(inputFiled);
            _tutorialService.Value.ShowFunnySlideOut(FunnySlideOutTexts.Step3);
            await _tutorialService.Value.WaitForInputFieldSelect(businessPreviewView.NameInputField);
            _tutorialService.Value.HideMask().HideArrow();
            var iconComponent = businessPreviewView.IconComponent;
            var iconRect = (RectTransform)iconComponent.transform;
            _tutorialService.Value.ShowArrow(iconRect).ShowMask(iconRect).AddArrowYOffset(100);
            await _tutorialService.Value.WaitForObservable(iconComponent.OnClick);
            _tutorialService.Value.HideTutorialView();
        }

        public void Hide()
        {
        }
            
    }
}