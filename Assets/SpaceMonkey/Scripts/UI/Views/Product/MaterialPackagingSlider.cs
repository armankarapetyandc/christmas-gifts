using R3;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class MaterialPackagingSlider : AbstractPriceSlider
    {
        [SerializeField] private float defaultSliderValue = 30f;
        public ReadOnlyReactiveProperty<float> CurrentValue => Model.Current;

        public void Setup(float packagingAddCoefficient, bool isPriorityCategory, float? currentValue = null)
        {
            float minPrice = packagingAddCoefficient * 0.6f + (isPriorityCategory ? 0.6f : 0f);
            float maxPrice = packagingAddCoefficient * 2.75f + (isPriorityCategory ? 2.75f : 0f);
            Model.Set(minPrice, maxPrice);
            Model.CalculateCurrent(slider.value);
            Setup();
            SetSliderValue(currentValue == null ? defaultSliderValue : Model.CalculateSliderValue(currentValue.Value));
        }

        // protected override void Setup()
        // {
        //     base.Setup();
        //     SetSliderValue(defaultSliderValue);
        // }

        public override void Reset()
        {
            SetSliderValue(defaultSliderValue);
        }
    }
}