using DG.Tweening;
using UIService.Runtime.Presenter.Animations;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Popups.Core
{
    [CreateAssetMenu(fileName = "PopupSlideAnimation", menuName = "UI Service/Animations/Popup Slide", order = 0)]
    public class PopupSlideAnimation : AbstractAnimation<PopupSlideAnimation.InputValues>
    {
        public override Sequence PlayOnShow(BasePresenter presenter)
        {
            Vector2 anchorPos = presenter.PresenterHolder.anchoredPosition;
            presenter.PresenterHolder.anchoredPosition = data.showValue.OutsidePoint(presenter.PresenterHolder, false);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, presenter.PresenterHolder.DOAnchorPos(anchorPos, data.showDuration)
                .SetDelay(data.delay)
                .SetEase(data.ease, data.showValue.overshoot));
            return sequence;
        }

        public override Sequence PlayOnHide(BasePresenter presenter)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, presenter.PresenterHolder
                .DOAnchorPos(data.hideValue.OutsidePoint(presenter.PresenterHolder, true), data.hideDuration)
                .SetDelay(data.delay)
                .SetEase(data.ease, data.hideValue.overshoot));
            return sequence;
        }

        [System.Serializable]
        public struct InputValues
        {
            public enum SlideDirection
            {
                LeftToRight,
                RightToLeft,
                BottomToTop,
                TopToBottom
            }

            public SlideDirection direction;
            public float overshoot;

            public Vector2 OutsidePoint(RectTransform holder, bool isHiding)
            {
                var point = direction switch
                {
                    SlideDirection.LeftToRight => holder.rect.width * Vector2.left,
                    SlideDirection.RightToLeft => holder.rect.width * Vector2.right,
                    SlideDirection.BottomToTop => holder.rect.height * Vector2.down,
                    SlideDirection.TopToBottom => holder.rect.height * Vector2.up
                };

                if (isHiding)
                    return -point;

                return point;
            }
        }
    }
}