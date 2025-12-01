using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class HotItemSubView : MonoBehaviour
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private RectTransform nextButtonRect;
        [SerializeField] private TextMeshProUGUI productNameText;
        [SerializeField] private IconComponent iconComponent;
        
        public RectTransform NextButtonRect => nextButtonRect;
        public Button NextButton => nextButton;
        public Button BackButton => backButton;

        public void Initialize(string productName,SpriteVisualAsset iconVisualAsset,ColorVisualAsset colorVisualAsset)
        {
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
            productNameText.SetText(productName);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}