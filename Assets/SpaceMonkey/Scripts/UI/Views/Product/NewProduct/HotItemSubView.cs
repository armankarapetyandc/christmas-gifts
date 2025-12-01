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
        [SerializeField] private Image productImage;
        [SerializeField] private TextMeshProUGUI productName;
        
        
        public RectTransform NextButtonRect => nextButtonRect;
        public Button NextButton => nextButton;
        public Button BackButton => backButton;


        public void Initialize(Sprite sprite, string pName)
        {
            productImage.sprite = sprite;
            productName.text = pName;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}