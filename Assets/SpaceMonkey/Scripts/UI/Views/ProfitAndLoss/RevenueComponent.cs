using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss.P_LComponents;
using TMPro;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class RevenueComponent : MonoBehaviour
    {
        [SerializeField] private PlProductComponent productComponentPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private TextMeshProUGUI totalCashText;

        [Inject] private AccountService _accountService;

        private PlProductComponent _productComponents;

        public void Initialize()
        {
            _productComponents = Instantiate(productComponentPrefab, container);
            _productComponents.ResetItemsList();
            foreach (var product in _accountService.Model.Account.Products)
            {
                _productComponents.AddItem(1, product.ProductPrice, product.Name);
            }
            _productComponents.TotalCost.Subscribe(total =>
                totalCashText.text = $"{total:F2}");
        }
    }
}