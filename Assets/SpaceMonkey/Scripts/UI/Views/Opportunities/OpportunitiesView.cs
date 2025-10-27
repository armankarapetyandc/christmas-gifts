using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities
{
    public class OpportunitiesView : BasePresenterWithController<OpportunitiesController>
    {
        [SerializeField] private Button bankAccountButton;
        [SerializeField] private Button creditCardButton;
        [SerializeField] private Button investmentButton;
        [SerializeField] private Button insuranceButton;
        [SerializeField] private Button mutualFundsButton;
        [SerializeField] private Button bigOrderButton;
        [SerializeField] private Button taxesButton;
        

        public override UniTask Initialize(IPresenterData data = null)
        {
            mutualFundsButton.OnClickAsObservable().Subscribe(_=>Controller.OnMutualFundsButtonClicked()).AddTo(this);
            creditCardButton.OnClickAsObservable().Subscribe(_=>Controller.OnCreditCardButtonClicked()).AddTo(this);
            investmentButton.OnClickAsObservable().Subscribe(_=>Controller.OnInvestmentButtonClicked()).AddTo(this);
            insuranceButton.OnClickAsObservable().Subscribe(_=>Controller.OnInsuranceButtonClicked()).AddTo(this);
            bankAccountButton.OnClickAsObservable().Subscribe(_=>Controller.OnBankAccountButtonClicked()).AddTo(this);
            bigOrderButton.OnClickAsObservable().Subscribe(_=>Controller.OnBigOrderButtonClicked()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}