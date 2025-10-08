using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.CreditCard;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.PayRoll;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class CashComponent : MonoBehaviour
    {
        [SerializeField] private NetProfitComponent expensesComponentPrefab;
        [SerializeField] private NetProfitComponent revenueComponentPrefab;
        [SerializeField] private TextMeshProUGUI totalCashText;
        [SerializeField] private Color profitColor;
        [SerializeField] private Color lossColor;

        [Inject] private AccountService _accountService;
        [Inject] private CreditSimulator _creditSimulator;
        

        public void Initialize(Dictionary<Profile.Product, int> week)
        {
            SetupRevenue(week);
            SetupExpenses(week);
            
            var profit = revenueComponentPrefab.TotalCost.Value - expensesComponentPrefab.TotalCost.Value;
            totalCashText.text = $"${profit:f2}";
            totalCashText.color = profit >= 0 ? profitColor : lossColor;
        }

        private void SetupExpenses(Dictionary<Profile.Product, int> weekExpenses)
        {
            // Products
            var totalMaterials = weekExpenses.Sum(pair => pair.Value * pair.Key.MaterialPrice);
            var totalPackaging = weekExpenses.Sum(pair => pair.Value * pair.Key.MaterialPackagingPrice);
            var totalShipping = weekExpenses.Sum(pair => pair.Value * pair.Key.ShippingCost);
            var totalProducts = totalMaterials + totalPackaging + totalShipping;

            if (totalProducts > 0)
            {
                expensesComponentPrefab.AddItem(1, totalProducts, "Product");
            }
            
            var totalPayroll = _accountService.Model.Account.Employees.Sum(employee => employee.Payroll);
            if (totalPayroll > 0)
            {
                // Payroll
                expensesComponentPrefab.AddItem(1, totalPayroll, "Payroll");
            }
            
            // Credit Card
            var creditCardPayment = _creditSimulator.GetPLPaymentAmount();
            if (creditCardPayment > 0)
            {
                expensesComponentPrefab.AddItem(1, creditCardPayment, "Credit Card Payment");   
            }
        }

        private void SetupRevenue(Dictionary<Profile.Product, int> week)
        {
            // Revenue
            revenueComponentPrefab.ResetItemsList(); 
            var totalProducts = week.Sum(pair => pair.Value * pair.Key.ProductPrice);
            if (totalProducts > 0)
            {
                revenueComponentPrefab.AddItem(1, totalProducts, "Product");
            }
        }
    }
}