using System.Collections.Generic;
using System.Linq;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ExpensesComponent : MonoBehaviour
    {
        [SerializeField] private PlProductComponent  productComponentPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private TextMeshProUGUI totalCashText;
        
        [Inject] private AccountService  _accountService;
        
        private readonly List<PlProductComponent> _productComponents =  new List<PlProductComponent>();

        public void Initialize()
        {
            foreach (var product in _accountService.Model.Account.Products)
            {
                var productComponent = Instantiate(productComponentPrefab, container);
                productComponent.SetData(product);
                _productComponents.Add(productComponent);
            }
            
            totalCashText.text = $"${_productComponents.Sum(item => item.TotalCost.CurrentValue):f2}";
        }
    }
}