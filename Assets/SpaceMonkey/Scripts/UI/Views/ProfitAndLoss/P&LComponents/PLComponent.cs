using System.Collections.Generic;
using System.Linq;
using R3;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents
{
    public abstract class PlComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private PLItem prefabItem;
        [SerializeField] private RectTransform container;
        [field: SerializeField] public List<PLItem> ItemsList { get; set; }
        
        public ReactiveProperty<float> TotalCost { get; } = new(0f);
        
        internal void SetComponentName(string componentName)
        {
            productNameText.text = componentName;
        }

        internal void ResetItemsList()
        {
            foreach (var item in ItemsList)
            {
                Destroy(item.gameObject);
            }
            ItemsList.Clear();
            CalculateTotalCost();
        }

        internal void AddItem(int count, float? cost, string componentName)
        {
            var item = Instantiate(prefabItem, container);
            item.SetItemData(count, cost, componentName);
            ItemsList.Add(item);
            CalculateTotalCost();
        }

        public virtual void SetData(object data)
        {
            CalculateTotalCost();
        }

        private void CalculateTotalCost()
        {
            if (ItemsList.Count == 0)
            {
                totalCostText.text = "$0.00";
                return;
            }
            var sum = ItemsList.Sum(item => item.Cost);
            TotalCost.Value = sum;
            totalCostText.text = $"${sum:f2}";
        }
    }
}