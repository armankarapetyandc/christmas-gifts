using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.DemoComplete
{
    public class DemoCompleteView : BasePresenterWithController<DemoCompleteController>
    {
        [SerializeField] private Button fillOutSurveyButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            fillOutSurveyButton.OnClickAsObservable().Subscribe(_=>Controller.SurveyButtonClicked()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}