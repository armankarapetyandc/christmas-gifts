using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class OverallTotalsComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI expensesText;
        [SerializeField] private TextMeshProUGUI revenueText;
        [SerializeField] private TextMeshProUGUI cashText;

        public void SetTotals(float expenses, float revenue, float cash)
        {
            expensesText.text = $"${expenses:F2}";
            revenueText.text = $"${revenue:F2}";
            cashText.text = $"${cash:F2}";
        }
    }
}