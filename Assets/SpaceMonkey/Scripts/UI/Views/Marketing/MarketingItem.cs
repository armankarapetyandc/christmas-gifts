using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingItem : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private MarketingSlider marketingSlider;
        
        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(SliderValueChanged).AddTo(this);
        }

        private void SliderValueChanged(bool selected)
        {
            marketingSlider.ChangeSliderActivation(selected);
        }
    }
}