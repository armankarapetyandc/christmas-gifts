using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Tutorial.Steps
{
    public class TutorialStep6 : ITutorialStep
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
            var saveRect = (RectTransform) saveButton.transform;
            _tutorialService.Value.ShowFunnySlideOut(FunnySlideOutTexts.Step61);
            // await UniTask.WaitWhile(() => businessHashtagsSelectionView.SaveButton.interactable == false);

            int countSelected = 0;
            string message = FunnySlideOutTexts.Step61;
            while (countSelected<16)
            {
                countSelected = await _tutorialService.Value.WaitForObservable(businessHashtagsSelectionView
                    .SelectedTagsCountChangedObservable);
                if (countSelected==10)
                {
                    message = FunnySlideOutTexts.Step62;
                }
                else if (countSelected == 16)
                {
                    message = FunnySlideOutTexts.Step63;
                }

                if (countSelected<16)
                {
                    _tutorialService.Value.ShowFunnySlideOut(message);
                }
                else
                {
                    _tutorialService.Value.ShowArrow(saveRect).ShowMask(saveRect)
                        .ShowFunnySlideOut(message);
                }
            }
           
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            _tutorialService.Value.HideArrow().HideMask();
        }

        public void Hide()
        {
        }
    }
}