using System;
using R3;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement
{
    public class CreditCardStatementView : BasePresenterWithController<CreditCardStatementView.Data, CreditCardStatementViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI minimumPaymentText;
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private Toggle skipPaymentToggle;
        [SerializeField] private Toggle payMinimumToggle;
        [SerializeField] private Toggle payFullToggle;
        [SerializeField] private GameObject transactionsContainer;
        [SerializeField] private TransactionItem transactionItemPrefab;
        [SerializeField] private TextMeshProUGUI creditScoreText;
        [SerializeField] private TextMeshProUGUI availableCreditText;
        [SerializeField] private TextMeshProUGUI paymentDueText;
        [SerializeField] private TextMeshProUGUI balanceAmountText;
        
        protected override void InternalInit()
        {
            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (PresenterData?.OnClose != null)
                {
                    Controller.CloseView();
                    PresenterData.OnClose.Invoke();
                    return;
                }
                Controller.OnBack();
            }).AddTo(this);
            skipPaymentToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    Controller.SelectPayment(PaymentOption.Skip);
                    SetToggleState();
                }
            });
            payMinimumToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    Controller.SelectPayment(PaymentOption.Minimum);
                    SetToggleState();
                }
            });
            payFullToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    Controller.SelectPayment(PaymentOption.Full);
                    SetToggleState();
                }
            });
            SetToggleState();
            InitTransactions();
            SetupTexts();
            
            minimumPaymentText.text = $"${Controller.minimumPayment:F2}";
            balanceText.text = $"${Controller.creditDataGameData.Balance:F2}";
        }

        private void SetToggleState()
        {
            skipPaymentToggle.isOn = Controller.creditDataGameData.SelectedPayment == PaymentOption.Skip;
            payMinimumToggle.isOn = Controller.creditDataGameData.SelectedPayment == PaymentOption.Minimum;
            payFullToggle.isOn = Controller.creditDataGameData.SelectedPayment == PaymentOption.Full;
        }

        private void InitTransactions()
        {
            foreach (var transaction in Controller.Transactions)
            {
                var item = Instantiate(transactionItemPrefab, transactionsContainer.transform);
                item.SetItemData(transaction);
            }
        }
        
        private void SetupTexts()
        {
            creditScoreText.text = Controller.creditDataGameData.CreditScore.ToString();
            availableCreditText.text = $"${Controller.creditDataGameData.CreditLimit:F2}";
            paymentDueText.text = $"Week {Controller.DueWeek}";
            balanceAmountText.text = $"${Controller.creditDataGameData.Balance:F2}";
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