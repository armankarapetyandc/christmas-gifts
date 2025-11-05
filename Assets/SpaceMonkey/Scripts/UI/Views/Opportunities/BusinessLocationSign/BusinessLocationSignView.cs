using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign
{
    public class BusinessLocationSignView : BasePresenterWithController<BusinessLocationSignViewController>
    {
        [SerializeField] private Button signButton;
        [SerializeField] private Button closeButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            signButton.OnClickAsObservable().Subscribe(_ => Controller.OnSign()).AddTo(this);
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            
        }
    }
}