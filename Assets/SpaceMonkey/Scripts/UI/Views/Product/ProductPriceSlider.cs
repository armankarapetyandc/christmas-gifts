using R3;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductPriceSlider : AbstractPriceSlider
    {
        [SerializeField] private TextMeshProUGUI profitText;
        [SerializeField] private float defaultSliderValue = 30f;

        private float _profit;
        public ReadOnlyReactiveProperty<float> CurrentValue => Model.Current;
        public float Profit => _profit;

        protected override void Setup()
        {
            base.Setup();
            Model.Current.Subscribe(value => profitText.text = $"${CalculateProfit(value):F2}").AddTo(this);

            SetSliderValue(defaultSliderValue);
        }

        public void Prepare()
        {
            Setup();
        }

        public void Prepare(float? minPrice, float? maxPrice, float? price)
        {
            if (minPrice == null || maxPrice == null || price == null)
            {
                Setup();
                return;
            }

            Model.Set(minPrice.Value, maxPrice.Value);
            Setup();
            SetSliderValue(Model.CalculateSliderValue(price.Value));
        }

        public void UpdateRange(float min, float max)
        {
            Model.Set(min, max);
            Model.CalculateCurrent(slider.value);
            SetSliderValue(Model.CalculateSliderValue(Model.Current.CurrentValue));
        }

        public float CalculateCurrentProfit()
        {
            return Model.Current.CurrentValue - Model.Minimum.CurrentValue;
        }
        
        private float CalculateProfit(float currentValue)
        {
            _profit = currentValue - Model.Minimum.CurrentValue;
            return _profit;
        }

        public override void Reset()
        {
            SetSliderValue(defaultSliderValue);
        }
    }
}