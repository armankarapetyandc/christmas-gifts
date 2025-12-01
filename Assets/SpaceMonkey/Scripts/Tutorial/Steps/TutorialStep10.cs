using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.BusinessHub;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep10 : ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 10;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }

        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<BusinessHubView>();
            var businessHubView = _presenterService.GetPresenter<BusinessHubView>();
            var productComponent = businessHubView.ProductComponent;
            var rectTransform = (RectTransform) productComponent.transform;
            _tutorialService.Value.ShowArrow(rectTransform).ShowMask(rectTransform)
                .ShowFunnySlideOut(FunnySlideOutTexts.Step101);
            await _tutorialService.Value.WaitForObservable(productComponent.OnClick);
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}