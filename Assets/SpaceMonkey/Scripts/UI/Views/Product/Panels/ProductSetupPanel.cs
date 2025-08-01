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
        
        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();


        private void Start()
        {
            
        }
    }
}