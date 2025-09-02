using System.Collections.Generic;
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

        public void Initialize(Dictionary<Profile.Product, int> week)
        {
            _productComponents = Instantiate(productComponentPrefab, container);
            _productComponents.ResetItemsList();

            foreach (KeyValuePair<Profile.Product, int> pair in week)
            {
                _productComponents.AddItem(pair.Value, pair.Key.ProductPrice, pair.Key.Name);
            }
            _productComponents.TotalCost.Subscribe(total =>
                totalCashText.text = $"{total:F2}").AddTo(this);
        }
    }
}