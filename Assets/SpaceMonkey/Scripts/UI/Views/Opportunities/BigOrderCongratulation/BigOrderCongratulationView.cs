using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation
{
    public class BigOrderCongratulationView : BasePresenterWithController<BigOrderCongratulationView.Data,BigOrderCongratulationViewController>
    {
        [SerializeField] private Button nextButton;

        protected override void InternalInit()
        {
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext(PresenterData.Type)).AddTo(this);
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