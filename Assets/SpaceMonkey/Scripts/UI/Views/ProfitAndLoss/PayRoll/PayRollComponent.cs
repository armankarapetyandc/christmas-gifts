using System.Collections.Generic;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.PayRoll
{
    public class PayRollComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI componentNameText;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private PayRollItem prefabItem;
        [SerializeField] private RectTransform container;
        public void Initialize(List<Employee> employees)
        {
            componentNameText.text = "Payroll";
            float totalCost = 0;
            foreach (var employee in employees)
            {
                totalCost += employee.Payroll;
            }
            totalCostText.text = $"${totalCost:F2}";
            AddItem(totalCost, "Salary");
            AddItem(0, "Benefits");
        }

        private void AddItem(float? cost, string componentName)
        {
            var item = Instantiate(prefabItem, container);
            item.SetItemData(cost, componentName);
        }
    }
}