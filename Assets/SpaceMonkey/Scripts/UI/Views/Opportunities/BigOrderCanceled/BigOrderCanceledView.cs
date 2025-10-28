using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCanceled
{
    public class BigOrderCanceledView : BasePresenterWithController<BigOrderCanceledView.Data,BigOrderCanceledViewController>
    {
        [SerializeField] private Button okButton;

        protected override void InternalInit()
        {
            okButton.OnClickAsObservable().Subscribe(_ => Controller.OnOkClicked()).AddTo(this);
        }

        public override void Dispose()
        {
        }
        public class Data : IPresenterData
        {
            public MainNavigationType Type { get; set; }
        }
    }
}