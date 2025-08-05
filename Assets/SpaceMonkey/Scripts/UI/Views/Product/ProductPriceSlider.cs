using R3;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductPriceSlider : AbstractPriceSlider
    {
        [SerializeField] private TextMeshProUGUI profitText;
        [SerializeField] private float defaultSliderValue = 30f;

        protected override void Setup()
        {
            base.Setup();
            Model.Current.Subscribe(value => profitText.text = $"${CalculateProfit(value):F2}").AddTo(this);

            SetSliderValue(Model.Maximum.CurrentValue <= Model.Minimum.CurrentValue ? 0f : defaultSliderValue);
        }

        public void Prepare()
        {
            Setup();
        }

        public void UpdateRange(float min, float max)
        {
            Model.Set(min, max);
            SetSliderValue(0f);
        }

        private float CalculateProfit(float currentValue)
        {
            return currentValue - Model.Minimum.CurrentValue;
        }
    }
}