using R3;
using SpaceMonkey.Scripts.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SpaceMonkey.Scripts.UI.Views.BusinessExamples
{
    public class BusinessExamplesItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button moreButton;

        private BusinessExample _businessExample;

        public Observable<BusinessExample> OnSelected => 
            moreButton.OnClickAsObservable().Select(_ => _businessExample);
        public string Category => _businessExample.Category;

        public void Setup(BusinessExample businessExample)
        {
            _businessExample = businessExample;
            UpdateUi();
        }

        private void UpdateUi()
        {
            nameText.text = _businessExample.Name;
        }
    }
}