using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessIconBuilder;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep4 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 4;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }

        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<BusinessIconBuilderView>();
            var businessPreviewView = _presenterService.GetPresenter<BusinessIconBuilderView>();
            var iconItem = businessPreviewView.IconCollectionComponent.FirstItem;
            var iconRect = (RectTransform) iconItem.transform;
            _tutorialService.Value.ShowArrow(iconRect).ShowMask(iconRect).ShowFunnySlideOut(FunnySlideOutTexts.Step41);
            await _tutorialService.Value.WaitForObservable(businessPreviewView.IconSelectedObservable);
            _tutorialService.Value.HideArrow().HideMask();

            var tabItem = businessPreviewView.ColorTab;
            var tabRect = (RectTransform) tabItem.transform;
            _tutorialService.Value.ShowArrow(tabRect).ShowMask(tabRect).ShowFunnySlideOut(FunnySlideOutTexts.Step42);;
            await _tutorialService.Value.WaitForObservable(tabItem.ToggleObservable);
            _tutorialService.Value.HideArrow().HideMask();

            var colorItem = businessPreviewView.ColorCollectionComponent.FirstItem;
            var colorRect = (RectTransform) colorItem.transform;
            _tutorialService.Value.ShowArrow(colorRect).ShowMask(colorRect).ShowFunnySlideOut(FunnySlideOutTexts.Step43);
            await _tutorialService.Value.WaitForObservable(businessPreviewView.ColorSelectedObservable);
            _tutorialService.Value.HideArrow().HideMask();

            var saveButton = businessPreviewView.SaveButton;
            var saveRect = (RectTransform) saveButton.transform;
            _tutorialService.Value.ShowArrow(saveRect).ShowMask(saveRect).ShowFunnySlideOut(FunnySlideOutTexts.Step44);
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}