using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.CreditCard
{
    public class CreditCardItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI costText;
        
        public void SetItemData(float? cost, string componentName = null)
        {
            if(!string.IsNullOrEmpty(componentName))componentNameText.text = componentName;
            if (cost == null) return;
            costText.text = $"${cost:F2}";
        }
    }
}