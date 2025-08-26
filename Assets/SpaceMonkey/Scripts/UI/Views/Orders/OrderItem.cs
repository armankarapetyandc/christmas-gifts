using System.Collections.Generic;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrderItem : MonoBehaviour
    {
        [SerializeField] private CharacterIconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI customerName;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productionCapacityText;
        [SerializeField] private Slider moodSlider;
        [SerializeField] private Button moreButton;
        [SerializeField] private RectTransform productsContainer;
        [SerializeField] private OrderProductItem orderProductItemPrefab;

        public void SetCharacterVisual(SpriteVisualAsset characterVisual = null,
            ColorVisualAsset baseColorVisual = null)
        {
            iconComponent.SetIcon(characterVisual);
            iconComponent.SetColor(baseColorVisual);
        }

        public void SetCustomerName(string name)
        {
            customerName.text = name;
        }

        public void SetMood(float value, SpriteVisualAsset moodVisual)
        {
            iconComponent.SetMoodIcon(moodVisual);
            moodSlider.value = value;
        }

        public void SetProducts(List<(ProductOrder productOrder, Profile.Product product, SpriteVisualAsset iconVisualAsset, ColorVisualAsset colorVisualAsset)> products)
        {
            foreach (var product in products)
            {
                var item = Instantiate(orderProductItemPrefab, productsContainer);
                item.Setup(product.iconVisualAsset, product.colorVisualAsset, product.productOrder.Quantity);
            }
        }
    }
}