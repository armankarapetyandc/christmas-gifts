using System;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement
{
    public class BusinessLoanStatementView: BasePresenterWithController<BusinessLoanStatementView.Data, BusinessLoanStatementViewController>
    {
        [Header("Header")]
        [SerializeField] private TextMeshProUGUI customerNameText;
        [SerializeField] private TextMeshProUGUI paymentDueText;
        [SerializeField] private TextMeshProUGUI dueDateText;
        
        [Header("Loan Details")]
        [SerializeField] private TextMeshProUGUI loanAmountText;
        [SerializeField] private TextMeshProUGUI interestRateText;
        [SerializeField] private TextMeshProUGUI maturityDateText;
        [SerializeField] private TextMeshProUGUI paymentsMadeText;
        
        [Header("Buttons")]
        [SerializeField] private Button backButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            UpdateView();
            return UniTask.CompletedTask;
        }

        protected override void InternalInit()
        {
            
        }

        private void UpdateView()
        {
            var (amount, apr, monthlyPayment, totalPayment) = Controller.GetLoanDetails();
            var customerName = Controller.GetCustomerName();
            var nextPaymentDate = Controller.GetNextPaymentDate();
            var maturityDate = Controller.GetMaturityDate();
            var paymentsMade = Controller.GetPaymentsMade();
            
            // Update header
            customerNameText.text = customerName;
            paymentDueText.text = $"{monthlyPayment:N2}";
            dueDateText.text = nextPaymentDate.ToString();
            
            // Update loan details
            loanAmountText.text = $"${amount:N2}";
            interestRateText.text = $"{apr:F1}%";
            maturityDateText.text = maturityDate.ToString();
            paymentsMadeText.text = paymentsMade.ToString();
        }

        public override void Dispose()
        {
            
        }
        
        public class Data : IPresenterData
        {
            public Action OnClose { get; set; }
        }
    }
}