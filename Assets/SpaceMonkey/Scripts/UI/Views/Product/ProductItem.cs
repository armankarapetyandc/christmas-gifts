using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI productName;
        [SerializeField] private TextMeshProUGUI prfitText;
        [SerializeField] private TextMeshProUGUI productDescription;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;

        public ProductData ProductData { get; private set; }
        

        public Observable<ProductData> Selected =>
            button.OnClickAsObservable().Select(_ => ProductData);

        public void Initialize(ProductData data)
        {
            ProductData = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            productName.text = ProductData.Name;
            prfitText.text = $"${ProductData.Profit:F2}";
            backgroundImage.color = ProductData.BackgroundColor;
            iconImage.sprite = ProductData.Icon;
        }
    }
}