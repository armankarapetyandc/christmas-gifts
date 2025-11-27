using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep6: ITutorialStep
    {
        private LazyInject<TutorialService> _tutorialService;
        private PresenterService _presenterService;
        public int Order => 6;

        [Inject]
        private void Inject(LazyInject<TutorialService> tutorialService, PresenterService presenterService)
        {
            _presenterService = presenterService;
            _tutorialService = tutorialService;
        }
        
        public async UniTask Show()
        {
            await _tutorialService.Value.WaitForWindowOpen<BusinessHashtagsSelectionView>();
            var businessHashtagsSelectionView = _presenterService.GetPresenter<BusinessHashtagsSelectionView>();
            businessHashtagsSelectionView.BackButton.enabled = false;
            
            var saveButton = businessHashtagsSelectionView.SaveButton;
            var saveRect = (RectTransform)saveButton.transform;
            
            await UniTask.WaitWhile(() => businessHashtagsSelectionView.SaveButton.interactable == false);
            _tutorialService.Value.ShowArrow(saveRect).ShowMask(saveRect);
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
            
        }

    }
}