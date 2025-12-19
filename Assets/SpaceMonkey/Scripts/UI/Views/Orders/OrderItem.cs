using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using SpaceMonkey.Scripts.Configs.Characters;
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
        [SerializeField] private RectTransform animationContainer;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private CharacterIconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI customerName;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productionCapacityText;
        [SerializeField] private Slider moodSlider;
        [SerializeField] private Button moreButton;
        [SerializeField] private Button shipButton;
        [SerializeField] private RectTransform productsContainer;
        [SerializeField] private OrderProductItem orderProductItemPrefab;
        [SerializeField] private LayoutElement layoutElement;

        public Observable<OrderItem> ShipOrder => shipButton.OnClickAsObservable().Select(_ => this);
        public WeekSimulationV2.Order Order { get; private set; }
        public int ProductionCapCost { get; private set; }
        public float Profit { get; private set; }

        public void SetCustomer(CharacterConfig customerCharacter)
        {
            customerName.text = customerCharacter.Name;
        }
        
        public void SetOrder(WeekSimulationV2.Order order)
        {
            Order = order;
        }

        public void SetCharacterVisual(SpriteVisualAsset characterVisual = null,
            ColorVisualAsset baseColorVisual = null)
        {
            iconComponent.SetIcon(characterVisual);
            iconComponent.SetColor(baseColorVisual);
        }

        public void SetMood(float value, SpriteVisualAsset moodVisual)
        {
            iconComponent.SetMoodIcon(moodVisual);
            moodSlider.value = value;
        }

        public void SetProducts(
            List<(WeekSimulationV2.OrderEntry productOrder, SpriteVisualAsset iconVisualAsset, ColorVisualAsset
                colorVisualAsset)> products)
        {
            foreach (var product in products)
            {
                var item = Instantiate(orderProductItemPrefab, productsContainer);
                item.Setup(product.iconVisualAsset, product.colorVisualAsset, product.productOrder.Quantity);
            }

            Calculate(products.Select(tuple => (tuple.productOrder.Product, tuple.productOrder.Quantity)).ToList());
        }

        private void Calculate(List<(Profile.Product product, int Quantity)> products)
        {
            float totalProdCapCost = 0f;
            float totalProfit = 0f;
            foreach (var tuple in products)
            {
                totalProdCapCost += tuple.product.ProdCapCost!.Value * tuple.Quantity;
                totalProfit += tuple.product.Profit!.Value * tuple.Quantity;
            }

            ProductionCapCost = Mathf.RoundToInt(totalProdCapCost);
            Profit = totalProfit;
            productionCapacityText.text = $"-{Mathf.Round(ProductionCapCost)} hr";
            moneyText.text = $"+${Profit:F2}";
        }

        public void SetInteractableState(bool state)
        {
            canvasGroup.interactable = state;
        }

        public UniTask SlideIn()
        {
            var elementHeight = layoutElement.preferredHeight;
            layoutElement.preferredHeight = 0;
            DOVirtual.Float(0, elementHeight, 0.5f, value => layoutElement.preferredHeight = value);
            animationContainer.anchoredPosition = new Vector2(Screen.width * 1.5f, 0f);
            return animationContainer.DOAnchorPos(Vector2.zero, 1f)
            .SetEase(Ease.InOutCubic).AsyncWaitForCompletion().AsUniTask();
        } 
    }
}