using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.BusinessHub;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep15: ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 15;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }

        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<BusinessHubView>();
            var hubView = _presenterService.GetPresenter<BusinessHubView>();
            var startButtonRect = (RectTransform)hubView.StartButton.transform;
            _tutorialService.Value.ShowArrow(startButtonRect).ShowMask(startButtonRect).AddArrowYOffset(-20);
            await _tutorialService.Value.WaitForObservable(hubView.StartButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}