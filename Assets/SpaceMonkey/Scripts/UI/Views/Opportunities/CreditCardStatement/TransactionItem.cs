using SpaceMonkey.Scripts.Simulation.CreditCard;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement
{
    public class TransactionItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI costText;

        public void SetItemData(Transaction transaction)
        {
            componentNameText.text = transaction.Description;
            costText.text = $"${transaction.Amount:F2}";
        }
    }
}