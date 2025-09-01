using R3;
using SpaceMonkey.Scripts.UI.Views.Product;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingSlider : AbstractPriceSlider
    {
        [SerializeField] private GameObject fillArea;
        [SerializeField] private GameObject handleSlideArea;
        [SerializeField] private float defaultSliderValue = 30f;

        public ReadOnlyReactiveProperty<float> CurrentValue => Model.Current;

        public void ChangeSliderActivation(bool isActive)
        {
            fillArea.SetActive(isActive);
            handleSlideArea.SetActive(isActive);
            slider.value = isActive ? defaultSliderValue : 0;
            if (!isActive) ResetCurrentValueText();
            slider.interactable = isActive;
        }

        public void Setup(float min, float max, float? currentValue)
        {
            Model.Set(min, max);
            Model.CalculateCurrent(slider.value);
            Setup();
            SetSliderValue(currentValue == null ? defaultSliderValue : Model.CalculateSliderValue(currentValue.Value));
        }
    }
}