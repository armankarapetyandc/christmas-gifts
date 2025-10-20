using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCanceled
{
    public class BigOrderCanceledView : BasePresenterWithController<BigOrderCanceledViewController>
    {
        [SerializeField] private Button okButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            okButton.OnClickAsObservable().Subscribe(_ => Controller.OnOkClicked()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}