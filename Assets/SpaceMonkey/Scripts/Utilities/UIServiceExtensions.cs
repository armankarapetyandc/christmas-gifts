using Cysharp.Threading.Tasks;
using DG.Tweening;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.Utilities
{
    public static class UIServiceExtensions
    {
        public static async UniTask HidePreviousAndShow<T>(this PresenterService service, IPresenterData data = null)
            where T : BasePresenter
        {
            await service.Hide();
            await service.Show<T>(data, hidePrevious: true);
        }

        public static async UniTask DOCrossfade(this CanvasGroup fromGroup, CanvasGroup toGroup, float duration,
            Ease ease = Ease.Linear)
        {
            // Kill any existing tweens for both groups
            fromGroup.DOKill();
            toGroup.DOKill();

            // Create a sequence to run both fades simultaneously
            Sequence crossfadeSequence = DOTween.Sequence();

            // Activate the target group and prepare it
            toGroup.gameObject.SetActive(true);
            toGroup.alpha = 0f;
            toGroup.interactable = true;
            toGroup.blocksRaycasts = true;

            // Add both tweens to run in parallel
            crossfadeSequence.Join(fromGroup.DOFade(0f, duration).SetEase(ease)
                .OnComplete(() =>
                {
                    fromGroup.interactable = false;
                    fromGroup.blocksRaycasts = false;
                    fromGroup.gameObject.SetActive(false);
                }));

            crossfadeSequence.Join(toGroup.DOFade(1f, duration).SetEase(ease));

            // Wait for completion
            await crossfadeSequence.AsyncWaitForCompletion();
        }
    }
}