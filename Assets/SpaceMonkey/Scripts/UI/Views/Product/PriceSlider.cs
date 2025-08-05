using System.Linq;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public enum  SliderType
    {
        Materials,
        Packaging,
        ProductPrice
    }

    public class SliderData
    {
        public ReactiveProperty<float> MinValue = new ReactiveProperty<float>(0f);
        public ReactiveProperty<float> MaxValue = new ReactiveProperty<float>(0f);
        public ReactiveProperty<float> CurrentValue = new ReactiveProperty<float>(0f);

        public void Initialize(SliderType type, Hashtag[] companyTags)
        {
            CalculateMinMaxValues(type, companyTags);
        }

        public void UpdatePrice(int sliderValue)
        {
            CurrentValue.Value = (sliderValue/100f)*(MaxValue.Value-MinValue.Value) + MinValue.Value;
        }
        
        private void CalculateMinMaxValues(SliderType type, Hashtag[] companyTags)
        {
            float tMAdd = companyTags.Sum(tag => tag.MaterialAdd);
            var caf = ProductExtensions.PriceCoefficients(type);
            MinValue.Value = type == SliderType.ProductPrice ? caf.Item1 * tMAdd + 0.35f : 0f;
            MaxValue.Value = type == SliderType.ProductPrice ? caf.Item2 * tMAdd + 5.5f : MinValue.Value * 3;
        }
    }
    public class PriceSlider : MonoBehaviour
    {
        [SerializeField] private SliderType sliderType;
        
        [SerializeField] private Slider slider;
        [SerializeField] private Image[] sliderFill;
        [SerializeField] private Button addPrice;
        [SerializeField] private Button deletePrice;
        
        [SerializeField] private TextMeshProUGUI minCostText;
        [SerializeField] private TextMeshProUGUI maxCostText;
        [SerializeField] private TextMeshProUGUI costText;
        
        
        private readonly ReactiveCommand<float> _changeCommand = new ReactiveCommand<float>();
        public Observable<float> PriceChangeCommand => _changeCommand;
        
        private SliderData _data = new SliderData();
        private int _sliderStep = 5;

        public void Initialize(Hashtag[] companyTags)
        {
            _data.Initialize(sliderType, companyTags);
            _data.MaxValue.Subscribe(value => maxCostText.text = $"${value.ToString("F2")}");
            _data.MinValue.Subscribe(value => minCostText.text = $"${value.ToString("F2")}");
            _data.CurrentValue.Subscribe(value =>
            {
                costText.text = $"${value.ToString("F2")}";
                _changeCommand.Execute(value);
            });
            addPrice.OnClickAsObservable().Subscribe(_ => { UpdateSliderValue(true);}).AddTo(this);
            deletePrice.OnClickAsObservable().Subscribe(_ => { UpdateSliderValue(false); }).AddTo(this);
            
            slider.minValue = 1;
            slider.maxValue = 100;
            slider.value = 30;
            _data.UpdatePrice((int)slider.value);
            // slider.OnValueChangedAsObservable().Subscribe(_ =>
            // {
            //     addPrice.interactable = slider.value < slider.maxValue;
            //     deletePrice.interactable = slider.value > slider.minValue;
            // }).AddTo(this);
        }

        private void UpdateSliderValue(bool addValue)
        {
            if (addValue)
            {
                slider.value =  Mathf.Clamp(slider.value + _sliderStep, slider.minValue,
                    slider.maxValue);
            }
            else
            {
                slider.value =  Mathf.Clamp(slider.value - _sliderStep, slider.minValue,
                    slider.maxValue);
            }
            _data.UpdatePrice((int)slider.value);
        }
    }
}