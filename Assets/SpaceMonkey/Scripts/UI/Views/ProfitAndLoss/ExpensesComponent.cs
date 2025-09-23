using System.Collections.Generic;
using System.Linq;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.PayRoll;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ExpensesComponent : MonoBehaviour
    {
        [SerializeField] private PlProductComponent productComponentPrefab;
        [SerializeField] private PayRollComponent payRollComponentPrefab;

        [SerializeField] private RectTransform container;
        [SerializeField] private TextMeshProUGUI totalCashText;
        
        [Inject] private AccountService  _accountService;
        
        private readonly List<PlProductComponent> _productComponents =  new List<PlProductComponent>();

        public void Initialize(Dictionary<Profile.Product, int> weekExpenses)
        {
            // Products
            foreach (KeyValuePair<Profile.Product, int> pair in weekExpenses)
            {
                var productComponent = Instantiate(productComponentPrefab, container);
                productComponent.SetData(pair);
                _productComponents.Add(productComponent);
            }
            // Payroll
            var payRollComponent = Instantiate(payRollComponentPrefab, container);
            payRollComponent.Initialize(_accountService.Model.Account.Employees);
            
            var productsTotal = _productComponents.Sum(item => item.TotalCost.CurrentValue);
            var payrollTotal = _accountService.Model.Account.Employees.Sum(employee => employee.Payroll);
            totalCashText.text = $"${productsTotal + payrollTotal:f2}";
        }
    }
}