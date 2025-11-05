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
        
        
        [Header("Last Month")]
        [SerializeField] private TextMeshProUGUI principalLastMonthText;
        [SerializeField] private TextMeshProUGUI interestLastMonthText;
        [SerializeField] private TextMeshProUGUI feeLastMonthText;
        [SerializeField] private TextMeshProUGUI totalLastMonthText;
        
        [Header("Year to date")]
        [SerializeField] private TextMeshProUGUI principalYearToDateText;
        [SerializeField] private TextMeshProUGUI interestYearToDateText;
        [SerializeField] private TextMeshProUGUI feeYearToDateText;
        [SerializeField] private TextMeshProUGUI totalYearToDateText;
        
        [Header("Buttons")]
        [SerializeField] private Button backButton;
        
        [SerializeField] private Button bottomInfoCloseButton;
        [SerializeField] private RectTransform bottomInfoContainer;
        
        
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            bottomInfoCloseButton.OnClickAsObservable().Subscribe(_ =>
            {
                PlayerPrefs.SetInt("BusinessLoanStatementInfoShown", 1);
                bottomInfoContainer.gameObject.SetActive(!PlayerPrefs.HasKey("BusinessLoanStatementInfoShown"));
            }).AddTo(this);
            bottomInfoContainer.gameObject.SetActive(!PlayerPrefs.HasKey("BusinessLoanStatementInfoShown"));
            
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
            var totalPaymentsCount = Controller.GetTotalPaymentsCount();
            
            // Update header
            customerNameText.text = customerName;
            paymentDueText.text = $"{monthlyPayment:N2}";
            dueDateText.text = nextPaymentDate.ToString();
            
            // Update loan details
            loanAmountText.text = $"${amount:N2}";
            interestRateText.text = $"{apr:F1}%";
            maturityDateText.text = $"Week {maturityDate}";
            paymentsMadeText.text = $"{paymentsMade} of {totalPaymentsCount}";
            
            // Update last month's payment data
            var (principal, interest, fee, total) = Controller.GetLastMonthPayment();
            principalLastMonthText.text = $"${principal:N2}";
            interestLastMonthText.text = $"${interest:N2}";
            feeLastMonthText.text = $"${fee:N2}";
            totalLastMonthText.text = $"${total:N2}";
            
            
            principalYearToDateText.text = $"${principal * 2:N2}";
            interestYearToDateText.text = $"${interest * 2:N2}";
            feeYearToDateText.text = $"${fee * 2:N2}";
            totalYearToDateText.text = $"${total * 2:N2}";
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