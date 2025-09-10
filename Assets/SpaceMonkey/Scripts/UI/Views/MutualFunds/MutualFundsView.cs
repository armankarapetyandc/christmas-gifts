using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.MutualFunds
{
    public class MutualFundsView : BasePresenterWithController<MutualFundsViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button investButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}