using System.Linq;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.Panels
{
    public class ProductSetupPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField productName;
        [SerializeField] private Button iconCreationButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;

        [SerializeField] private TimeToProduceSlider timeToProductSlider;
        [SerializeField] private MaterialPriceSlider materialPriceSlider;
        [SerializeField] private MaterialPackagingSlider materialPackagingSlider;
        [SerializeField] private ProductPriceSlider productPriceSlider;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI shippingCostText;

        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();

        [Inject] private AccountService _accountService;


        public void Start()
        {
            timeToProductSlider.Setup(Constants.TimeToProduct, 1);

            bool isPriorityCategory = _accountService.Model.Account.Company.Category == "Cooking";
            float materialAddCoefficient = _accountService.Model.Account.Company.Tags.Select(t => t.MaterialAdd).Sum();
            materialPriceSlider.Setup(materialAddCoefficient, isPriorityCategory);

            float packagingAddCoefficient =
                _accountService.Model.Account.Company.Tags.Select(t => t.PackagingAdd).Sum();
            materialPackagingSlider.Setup(packagingAddCoefficient, isPriorityCategory);


            productPriceSlider.Prepare();

            Observable.Merge(
                timeToProductSlider.CurrentValue.Select(_ => Unit.Default),
                materialPriceSlider.CurrentValue.Select(_ => Unit.Default),
                materialPackagingSlider.CurrentValue.Select(_ => Unit.Default)
            ).Subscribe(_ =>
            {
                var ttpCost = timeToProductSlider.CurrentValue.CurrentValue *
                              (materialPriceSlider.CurrentValue.CurrentValue +
                               materialPackagingSlider.CurrentValue.CurrentValue * 0.5f);

                var totalCost = materialPriceSlider.CurrentValue.CurrentValue +
                                materialPackagingSlider.CurrentValue.CurrentValue +
                                ttpCost;

                totalCostText.text = $"${totalCost:F2}";

                var shippingCost = materialPackagingSlider.CurrentValue.CurrentValue * 0.1f + totalCost * 0.1f + 1.2f;
                shippingCostText.text = $"+${shippingCost:F2}";

                var productMaxPrice = totalCost * 3f;
                productPriceSlider.UpdateRange(totalCost, productMaxPrice);
            }).AddTo(this);
        }


        // public void Initialize(AccountModel accountServiceModel)
        // {
        //     _accountModel = accountServiceModel;
        //     InitTtpSlider();
        // }
        //
        // private void InitTtpSlider()
        // {
        //     timeToProductAbstractPriceSlider.value = 0;
        //     tickMarks[(int)timeToProductAbstractPriceSlider.value].color = activeTickMarkColor;
        //     timeToProductAbstractPriceSlider.minValue = 0;
        //     timeToProductAbstractPriceSlider.maxValue = 4;
        //     slowerButton.onClick.AddListener(() => ChangeStep(-1));
        //     fasterButton.onClick.AddListener(() => ChangeStep(1));
        //     timeToProductAbstractPriceSlider.OnValueChangedAsObservable().Subscribe(_ =>
        //     {
        //         slowerButton.interactable = timeToProductAbstractPriceSlider.value > 0;
        //         fasterButton.interactable = timeToProductAbstractPriceSlider.value < 4;
        //     });
        // }
        //
        // private void ChangeStep(int direction)
        // {
        //     timeToProductAbstractPriceSlider.value = Mathf.Clamp(timeToProductAbstractPriceSlider.value + direction, timeToProductAbstractPriceSlider.minValue,
        //         timeToProductAbstractPriceSlider.maxValue);
        //     UpdateLabel();
        // }
        //
        // private void UpdateLabel()
        // {
        //     if (timeToProductLabelText != null && timeToProductAbstractPriceSlider.value >= 0 &&
        //         timeToProductAbstractPriceSlider.value <= timeToProductAbstractPriceSlider.maxValue)
        //     {
        //         var ttp = Constants.TimeToProduct.ElementAt((int)timeToProductAbstractPriceSlider.value);
        //         timeToProductLabelText.text = ttp.Key;
        //         tickMarks.Select((img, i) => new { img, i })
        //             .ToList()
        //             .ForEach(t =>
        //                 t.img.color = (t.i == (int)timeToProductAbstractPriceSlider.value)
        //                     ? activeTickMarkColor
        //                     : defaultTickMarkColor);
        //         _ttpCoefficient = ttp.Value;
        //     }
        // }
        //
        // public void SetProductIcon(Sprite icon, Color color)
        // {
        //     backgroundImage.gameObject.SetActive(true);
        //     backgroundImage.color = color;
        //     iconImage.sprite = icon;
        // }
        //
        // private void CalculateTotalCost()
        // {
        //     _costs.TotalCost.Value = _costs.MaterialCost.Value + _costs.PackagingCost.Value + _ttpCoefficient;
        // }
        //
        // private void CalculateTtpCost()
        // {
        //     var prodCost = _costs.MaterialCost.Value + 0.5f * _costs.PackagingCost.Value;
        //     _costs.TtpCost.Value = _ttpCoefficient + prodCost;
        // }
    }
}