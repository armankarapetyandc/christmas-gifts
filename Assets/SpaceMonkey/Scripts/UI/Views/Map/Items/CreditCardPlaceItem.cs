using SpaceMonkey.Scripts.Simulation.CreditCard;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Map.Items
{
    public class CreditCardPlaceItem : MapPlaceHolderItem
    {
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private TextMeshProUGUI nextPaymentText;
        [SerializeField] private TextMeshProUGUI dueDateText;
        private CreditSimulator _creditSimulator;

        [Inject]
        private void Inject(CreditSimulator creditSimulator)
        {
            _creditSimulator = creditSimulator;
        }
        
        public override void Init()
        {
            float balance = 0;
            float nextPayment = 0;
            int dueDate = 0;
            
            SetLocked(!_creditSimulator.HasActiveCard);
            if (_creditSimulator.HasActiveCard)
            {
                balance = _creditSimulator.Data.Balance;
                nextPayment = _creditSimulator.GetPaymentAmount();
                dueDate = _creditSimulator.DueWeek;
            }
            
            balanceText.text = $"Balance: ${balance:F2}";
            nextPaymentText.text = $"Next Payment: ${nextPayment:F2}";
            dueDateText.text = $"Payment Due: Week {dueDate}";
            base.Init();
        }
    }
}