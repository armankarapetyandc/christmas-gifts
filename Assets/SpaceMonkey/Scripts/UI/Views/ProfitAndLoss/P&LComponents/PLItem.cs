using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents
{
    public class PLItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private TextMeshProUGUI costText;
        public float Cost { get; private set; }

        public void SetItemData(int count, float? cost, string componentName = null)
        {
            if(!string.IsNullOrEmpty(componentName))componentNameText.text = componentName;
            countText.text= $"({cost:f2}x{count})";
            if (cost == null) return;
            Cost = (float)(count * cost);
            costText.text = $"${Cost:F2}";
        }
    }
}