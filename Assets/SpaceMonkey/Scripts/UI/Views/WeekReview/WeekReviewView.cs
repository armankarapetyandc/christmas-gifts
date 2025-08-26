using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.WeekReview
{
    public class WeekReviewView : BasePresenterWithController<WeekReviewController>
    {
        [SerializeField] private Button nextButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}