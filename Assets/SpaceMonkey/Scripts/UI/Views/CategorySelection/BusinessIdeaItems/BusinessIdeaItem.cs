using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.CategorySelection.BusinessIdeaItems
{
    public class BusinessIdeaItem : MonoBehaviour
    {
        [SerializeField] private Image businessIdeaImage;
        [SerializeField] private TextMeshProUGUI businessIdeaName;
        [SerializeField] private Button categoryButton;
        private BusinessItemData _data;

        public Observable<BusinessItemData> OnClickObservable => categoryButton.OnClickAsObservable().Select(_ => _data);
        

        public void Initialize( BusinessItemData data)
        {
            _data = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            businessIdeaName.text = _data.BusinessName;
            businessIdeaImage.sprite = _data.BusinessIcon;
        }
    }
}