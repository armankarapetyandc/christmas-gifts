using System.Collections.Generic;
using System.Threading;
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

            CancellationTokenSource cts = new CancellationTokenSource();
            HandleMassagesUpdate(businessHashtagsSelectionView, saveRect, cts.Token).Forget();
            await _tutorialService.Value.WaitForObservable(saveButton.OnClickAsObservable());
            cts.Cancel();
            
            _tutorialService.Value.HideArrow().HideMask();
        }

        private async UniTask HandleMassagesUpdate(BusinessHashtagsSelectionView businessHashtagsSelectionView,
            RectTransform saveRect, CancellationToken cancellationToken)
        {
            int countSelected = 0;
            string message = FunnySlideOutTexts.Step61;
            while (countSelected<16)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                
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

                await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            }
        }

        public void Hide()
        {
        }
    }
}