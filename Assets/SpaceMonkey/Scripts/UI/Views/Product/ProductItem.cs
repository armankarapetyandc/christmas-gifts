using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI productName;
        [SerializeField] private TextMeshProUGUI productPrice;
        [SerializeField] private TextMeshProUGUI productDescription;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        
        private ProductData _data;
        
        public Observable<ProductData> Selected => button.OnClickAsObservable().Select(_ => _data);

        public void Initialize(ProductData data)
        {
            _data = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            productName.text = _data.ProductName;
            productPrice.text = _data.ProductPrice;
            productDescription.text = _data.ProductDescription;
            backgroundImage.color = _data.BackgroundColor;
            iconImage.sprite = _data.ProductSprite;
        }
    }
}