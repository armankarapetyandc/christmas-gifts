using System;
using R3;
using SpaceMonkey.Scripts.Simulation.CreditCard;
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

            minimumPaymentText.text = $"${Controller.minimumPayment}";
            balanceText.text = $"${Controller.gameData.Balance}";
        }

        private void SetToggleState()
        {
            skipPaymentToggle.isOn = Controller.gameData.SelectedPayment == PaymentOption.Skip;
            payMinimumToggle.isOn = Controller.gameData.SelectedPayment == PaymentOption.Minimum;
            payFullToggle.isOn = Controller.gameData.SelectedPayment == PaymentOption.Full;
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