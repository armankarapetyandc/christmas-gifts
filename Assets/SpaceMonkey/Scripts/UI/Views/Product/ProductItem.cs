using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI productName;

        [FormerlySerializedAs("prfitText")] [SerializeField]
        private TextMeshProUGUI profitText;

        [SerializeField] private TextMeshProUGUI productDescription;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        private Profile.Product _product;
        private SpriteVisualAsset _spriteVisualAsset;
        private ColorVisualAsset _colorVisualAsset;

        public Observable<Profile.Product> Selected => button.OnClickAsObservable().Select(_ => _product);

        public void Setup(Profile.Product product, SpriteVisualAsset spriteVisualAsset,
            ColorVisualAsset colorVisualAsset)
        {
            _colorVisualAsset = colorVisualAsset;
            _spriteVisualAsset = spriteVisualAsset;
            _product = product;
            UpdateUI();
        }

        private void UpdateUI()
        {
            productName.text = _product.Name;
            profitText.text = $"${_product.Profit:F2}";
            backgroundImage.color = _colorVisualAsset?.Color ?? Color.white;
            iconImage.sprite = _spriteVisualAsset?.Sprite;
        }
    }
}