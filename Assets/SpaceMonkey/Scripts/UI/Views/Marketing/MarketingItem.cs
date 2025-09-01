using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingItem : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private MarketingSlider marketingSlider;
        
        public MarketingSlider MarketingSlider => marketingSlider;
        public bool IsSelected => toggle.isOn;

        private void Start()
        {
            if (toggle != null)
                toggle.OnValueChangedAsObservable().Subscribe(SliderValueChanged).AddTo(this);
        }

        private void SliderValueChanged(bool selected)
        {
            if (marketingSlider != null)
                marketingSlider.ChangeSliderActivation(selected);
        }
    }
}