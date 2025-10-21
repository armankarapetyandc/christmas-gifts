using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.DisasterInsurance
{
    public class DisasterInsuranceView : BasePresenterWithController<DisasterInsuranceViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button purchasePolicyButton;
        [SerializeField] private Button learnAboutInsuranceButton;
        [SerializeField] private TextMeshProUGUI moneyText;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            purchasePolicyButton.OnClickAsObservable().Subscribe(_ => Controller.PurchasePolicy()).AddTo(this);
            learnAboutInsuranceButton.OnClickAsObservable().Subscribe(_ => Controller.LearnAboutInsurance())
                .AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}