using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product.Panels
{
    public class ProductSetupPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField productName;
        [SerializeField] private Button iconCreationButton;
        [SerializeField] private Button saveButton;
        
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        
        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();

        public void SetProductIcon(Sprite icon, Color color)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = color;
            iconImage.sprite = icon;
        }
    }
}