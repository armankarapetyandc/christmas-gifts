using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration;
using SpaceMonkey.Scripts.UI.Views.Map;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep9: ITutorialStep
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
            await _tutorialService.Value.WaitForWindowOpen<MapView>();
            var mapView = _presenterService.GetPresenter<MapView>();
            mapView.SelectMyPlace();


        }

        public void Hide()
        {
            
        }

    }
}