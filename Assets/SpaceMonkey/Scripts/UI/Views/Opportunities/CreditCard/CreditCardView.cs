using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard
{
    public class CreditCardView : BasePresenterWithController<CreditCardViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button applyButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            applyButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}