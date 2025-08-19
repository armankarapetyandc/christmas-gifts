using System.Collections.Generic;
using System.Linq;
using R3;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class TimeToProduceSlider : AbstractPriceSlider
    {
        [SerializeField] private TextMeshProUGUI labelText;
        private IDictionary<string, float> _states;

        public ReadOnlyReactiveProperty<float> CurrentValue => Model.Current;

        protected override void Setup()
        {
            base.Setup();
            Model.Current.Subscribe(value =>
            {
                var index = Mathf.RoundToInt(value);
                var state = _states.ElementAtOrDefault(index);
                labelText.text = state.Key;
            }).AddTo(this);
        }

        public void Setup(IDictionary<string, float> states, int currentIndex)
        {
            _states = states;
            Model.Set(0, states.Count - 1);
            Model.CalculateCurrent(slider.value);
            Setup();
            SetSliderValue(Model.CalculateSliderValue(currentIndex));
        }
        
        public override void Reset()
        {
            SetSliderValue(0);
        }
    }
}