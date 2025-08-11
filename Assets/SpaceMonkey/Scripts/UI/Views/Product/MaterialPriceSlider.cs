using R3;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class MaterialPriceSlider : AbstractPriceSlider
    {
        [SerializeField] private float defaultSliderValue = 30f;

        public ReadOnlyReactiveProperty<float> CurrentValue => Model.Current;

        public void Setup(float materialAddCoefficient, bool isPriorityCategory)
        {
            float minPrice = materialAddCoefficient * 0.35f + (isPriorityCategory ? 0.35f : 0f);
            float maxPrice = materialAddCoefficient * 5.5f + (isPriorityCategory ? 5.5f : 0f);
            Model.Set(minPrice, maxPrice);
            Setup();
        }

        protected override void Setup()
        {
            base.Setup();
            SetSliderValue(defaultSliderValue);
        }
 
        public override void Reset()
        {
            SetSliderValue(defaultSliderValue);
        }
    }
}