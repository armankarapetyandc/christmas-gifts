using R3;
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
        private Sprite _iconSprite;

        public Observable<Profile.Product> Selected => button.OnClickAsObservable().Select(_ => _product);

        public void Setup(Profile.Product product, Sprite productIcon)
        {
            _iconSprite = productIcon;
            _product = product;
            UpdateUI();
        }

        private void UpdateUI()
        {
            productName.text = _product.Name;
            profitText.text = $"${_product.Profit:F2}";
            if (ColorUtility.TryParseHtmlString(_product.BackgroundColor, out var color))
            {
                backgroundImage.color = color;
            }

            iconImage.sprite = _iconSprite;
        }
    }
}