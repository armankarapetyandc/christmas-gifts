using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement
{
    public class TransactionItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI costText;

        public void SetItemData(PaymentRecord transaction)
        {
            componentNameText.text = transaction.Description;
            costText.text = $"${transaction.Payment:F2}";
        }
    }
}