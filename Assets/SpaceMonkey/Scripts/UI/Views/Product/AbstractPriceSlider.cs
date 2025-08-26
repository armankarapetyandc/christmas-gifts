using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public abstract class AbstractPriceSlider : MonoBehaviour
    {
        public class DataModel
        {
            private readonly ReactiveProperty<float> _minimum = new ReactiveProperty<float>();
            private readonly ReactiveProperty<float> _maximum = new ReactiveProperty<float>();
            private readonly ReactiveProperty<float> _current = new ReactiveProperty<float>();

            public ReadOnlyReactiveProperty<float> Minimum => _minimum;
            public ReadOnlyReactiveProperty<float> Maximum => _maximum;
            public ReadOnlyReactiveProperty<float> Current => _current;

            public void Set(float min, float max)
            {
                _minimum.Value = min;
                _maximum.Value = max;
                _current.Value = min; // Initialize current to minimum value
            }
            public void CalculateCurrent(float sliderValue)
            {
                if (_maximum.CurrentValue <= _minimum.CurrentValue)
                {
                    _current.Value = _minimum.CurrentValue;
                    return;
                }

                var result = (sliderValue / 100f) * (_maximum.CurrentValue - _minimum.CurrentValue) +
                             _minimum.CurrentValue;
                _current.Value = result;
            }

            public float CalculateSliderValue(float currentValue)
            {
                return ((currentValue - _minimum.Value) / (_maximum.CurrentValue - _minimum.CurrentValue)) * 100f;
            }
        }

        [SerializeField] protected Slider slider;
        [SerializeField] private Button increaseButton;
        [SerializeField] private Button decreaseButton;
        [SerializeField] private TextMeshProUGUI minText;
        [SerializeField] private TextMeshProUGUI maxText;
        [SerializeField] private TextMeshProUGUI currentValueText;
        [SerializeField] private float sliderStep = 5f;

        protected DataModel Model { get; private set; } = new();

        protected virtual void Setup()
        {
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.value = 0f;

            if (minText != null)
            {
                Model.Minimum.Subscribe(value => minText.text = $"${value:F2}").AddTo(this);
            }

            if (maxText != null)
            {
                Model.Maximum.Subscribe(value => maxText.text = $"${value:F2}").AddTo(this);
            }

            if (currentValueText != null)
            {
                Model.Current.Subscribe(value => currentValueText.text = $"${value:F2}").AddTo(this);
            }

            slider.OnValueChangedAsObservable().Subscribe(value => Model.CalculateCurrent(value)).AddTo(this);
            if(increaseButton != null) increaseButton.OnClickAsObservable().Subscribe(_ => ChangeSliderValue(sliderStep)).AddTo(this);
            if(decreaseButton != null) decreaseButton.OnClickAsObservable().Subscribe(_ => ChangeSliderValue(-sliderStep)).AddTo(this);
        }

        public virtual void Reset(){}

        private void ChangeSliderValue(float amount)
        {
            SetSliderValue(Mathf.Clamp(slider.value + amount, slider.minValue, slider.maxValue));
        }

        protected void SetSliderValue(float value)
        {
            slider.value = value;
        }

        public float GetSliderValue()
        {
            return slider.value;
        }
    }
}