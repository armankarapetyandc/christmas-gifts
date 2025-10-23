using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoan
{
    public class BusinessLoanView :  BasePresenterWithController<BusinessLoanViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button applyButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button learMoreButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            applyButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => Controller.OnInfo()).AddTo(this);
            learMoreButton.OnClickAsObservable().Subscribe(_ => Controller.OnInfo()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            
        }
    }
}