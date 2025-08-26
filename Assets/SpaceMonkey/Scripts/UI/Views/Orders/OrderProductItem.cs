using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrderProductItem : MonoBehaviour
    {
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI quantityText;

        public void Setup(SpriteVisualAsset iconVisualAsset, ColorVisualAsset colorVisualAsset, int quantity)
        {
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
            quantityText.text = quantity.ToString();
        }
    }
}