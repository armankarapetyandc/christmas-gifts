using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.PayRoll;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.CreditCard
{
    public class CreditCardComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private CreditCardItem prefabItem;
        [SerializeField] private RectTransform container;
        
        public void Initialize(float payment)
        {
            componentNameText.text = "Recurring Payments";
            totalCostText.text = $"${payment:F2}";
            var item = Instantiate(prefabItem, container);
            item.SetItemData(payment, "Credit Card (Fweep Bank)");
        }
    }
}