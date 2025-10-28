using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Map.Items
{
    public class BusinessLoanHolderItem : MapPlaceHolderItem
    {
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private TextMeshProUGUI nextPaymentText;
        [SerializeField] private TextMeshProUGUI dueDateText;
        private BusinessLoanSimulator _businessLoanSimulator;

        [Inject]
        private void Inject(BusinessLoanSimulator businessLoanSimulator)
        {
            _businessLoanSimulator = businessLoanSimulator;
        }
        
        public override void Init(PlaceType placeType)
        {
            float balance = 0;
            float nextPayment = 0;
            int dueDate = 0;
            
            SetLocked(!_businessLoanSimulator.HasActiveLoan);
            if (_businessLoanSimulator.HasActiveLoan)
            {
                balance = _businessLoanSimulator.Data.Balance;
                nextPayment = _businessLoanSimulator.GetNextPaymentAmount();
                dueDate = _businessLoanSimulator.GetNextPaymentDate();
            }
            
            balanceText.text = $"Balance: ${balance:F2}";
            nextPaymentText.text = $"Next Payment: ${nextPayment:F2}";
            dueDateText.text = $"Payment Due: Week {dueDate}";
            base.Init(placeType);
        }
    }
}