using System;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI productName;
        [SerializeField] private TextMeshProUGUI profitText;

        [SerializeField] private TextMeshProUGUI productDescription;
        [SerializeField] private IconComponent iconComponent;

        [SerializeField] private Button button;
        private Profile.Product _product;
        private SpriteVisualAsset _spriteVisualAsset;
        private ColorVisualAsset _colorVisualAsset;

        public Observable<Profile.Product> Selected => button.OnClickAsObservable().Select(_ => _product);

        public void Setup(Profile.Product product)
        {
            _product = product;
            productName.text = _product.Name;
            // profitText.text = $"${_product.Profit:F2}";
        }

        public void SetVisual(SpriteVisualAsset iconVisualAsset, ColorVisualAsset colorVisualAsset)
        {
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
        }
    }
}