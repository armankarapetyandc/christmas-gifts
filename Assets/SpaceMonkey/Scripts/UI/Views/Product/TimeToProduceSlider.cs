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
        private int _currentIndex;

        public ReadOnlyReactiveProperty<float> CurrentValue => Model.Current;
        public int CurrentIndex => _currentIndex;

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
            _currentIndex = currentIndex;
            Model.Set(0, states.Count - 1);
            var percent = ((currentIndex - Model.Minimum.CurrentValue) /
                           (Model.Maximum.CurrentValue - Model.Minimum.CurrentValue)) * 100f;
            Setup();
            SetSliderValue(percent);
        }
        
        public override void Reset()
        {
            SetSliderValue(0);
        }
    }
}