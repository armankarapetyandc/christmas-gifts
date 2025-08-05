using System.Linq;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product.Panels
{
    public class ProductSetupPanel : MonoBehaviour
    {
        private class Costs
        {
            public ReactiveProperty<float> MaterialCost = new ReactiveProperty<float>(0f);
            public ReactiveProperty<float> PackagingCost = new ReactiveProperty<float>(0f);
            public ReactiveProperty<float> TotalCost = new ReactiveProperty<float>(0f);
            public ReactiveProperty<float> TtpCost = new ReactiveProperty<float>(0f);
            public ReactiveProperty<float> ProductCost = new ReactiveProperty<float>(0f);
        }
        
        [SerializeField] private TMP_InputField productName;
        [SerializeField] private Button iconCreationButton;
        [SerializeField] private Button saveButton;

        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;

        [SerializeField] private Slider timeToProductSlider;
        [SerializeField] private TextMeshProUGUI timeToProductLabelText;
        [SerializeField] private Button fasterButton;
        [SerializeField] private Button slowerButton;
        [SerializeField] private Image[] tickMarks;
        [SerializeField] private Color defaultTickMarkColor;
        [SerializeField] private Color activeTickMarkColor;

        [SerializeField] private PriceSlider materialsSlider;
        [SerializeField] private PriceSlider packagingSlider;
        [SerializeField] private PriceSlider productSlider;
        
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI ppddText;

        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();

        private float _ttpCoefficient;
        private AccountModel _accountModel;
        private Costs _costs = new Costs();

        public void Initialize(AccountModel accountServiceModel)
        {
            _accountModel = accountServiceModel;
            InitTtpSlider();
            materialsSlider.Initialize(_accountModel.Account.Company.Tags);
            packagingSlider.Initialize(_accountModel.Account.Company.Tags);
            materialsSlider.PriceChangeCommand.Subscribe(value =>
            {
                _costs.MaterialCost.Value = value;
                CalculateTotalCost();
            });
            packagingSlider.PriceChangeCommand.Subscribe(value =>
            {
                _costs.PackagingCost.Value = value;
                CalculateTtpCost();
            });
            productSlider.PriceChangeCommand.Subscribe(value =>
            {
                _costs.ProductCost.Value = value;
            });
            _costs.TotalCost.Subscribe(value => totalCostText.text = $"${value.ToString("F2")}");
            _costs.TtpCost.Subscribe(value => ppddText.text = $"+${value.ToString("F2")}");
        }

        private void InitTtpSlider()
        {
            timeToProductSlider.value = 0;
            tickMarks[(int)timeToProductSlider.value].color = activeTickMarkColor;
            timeToProductSlider.minValue = 0;
            timeToProductSlider.maxValue = 4;
            slowerButton.onClick.AddListener(() => ChangeStep(-1));
            fasterButton.onClick.AddListener(() => ChangeStep(1));
            timeToProductSlider.OnValueChangedAsObservable().Subscribe(_ =>
            {
                slowerButton.interactable = timeToProductSlider.value > 0;
                fasterButton.interactable = timeToProductSlider.value < 4;
            });
        }

        private void ChangeStep(int direction)
        {
            timeToProductSlider.value = Mathf.Clamp(timeToProductSlider.value + direction, timeToProductSlider.minValue,
                timeToProductSlider.maxValue);
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (timeToProductLabelText != null && timeToProductSlider.value >= 0 &&
                timeToProductSlider.value <= timeToProductSlider.maxValue)
            {
                var ttp = Constants.TimeToProduct.ElementAt((int)timeToProductSlider.value);
                timeToProductLabelText.text = ttp.Key;
                tickMarks.Select((img, i) => new { img, i })
                    .ToList()
                    .ForEach(t =>
                        t.img.color = (t.i == (int)timeToProductSlider.value)
                            ? activeTickMarkColor
                            : defaultTickMarkColor);
                _ttpCoefficient = ttp.Value;
            }
        }

        public void SetProductIcon(Sprite icon, Color color)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = color;
            iconImage.sprite = icon;
        }

        private void CalculateTotalCost()
        {
            _costs.TotalCost.Value = _costs.MaterialCost.Value + _costs.PackagingCost.Value + _ttpCoefficient;
        }

        private void CalculateTtpCost()
        {
            var prodCost = _costs.MaterialCost.Value + 0.5f * _costs.PackagingCost.Value;
            _costs.TtpCost.Value = _ttpCoefficient + prodCost;
        }
    }
}