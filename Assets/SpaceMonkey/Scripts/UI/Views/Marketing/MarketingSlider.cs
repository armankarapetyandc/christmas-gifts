using System;
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
            if (!isActive)
            {
                ResetCurrentValueText();
            }
            slider.interactable = isActive;
        }

        public void Setup(float minMult, float maxMult, int level, string sliderType, float? currentValue, bool isActive = true)
        {
            var (min, max) = sliderType switch
            {
                "Flyer"  => (minMult * level, maxMult * level),
                "Social" => (minMult * level, maxMult * (level * level)),
                "Email"  => (minMult * (level * 1.5f), maxMult * (level * 1.5f)),
                _        => throw new ArgumentException($"Unknown slider type: {sliderType}")
            };

            Model.Set(min, max);
            Model.CalculateCurrent(slider.value);

            Setup();

            var valueToSet = currentValue.HasValue 
                ? Model.CalculateSliderValue(currentValue.Value) 
                : defaultSliderValue;

            SetSliderValue(valueToSet);
            if(!isActive) ResetCurrentValueText();
        }

    }
}