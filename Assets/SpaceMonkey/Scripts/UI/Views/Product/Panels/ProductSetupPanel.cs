using System.Linq;
using NameGenerator.Generators;
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
        [SerializeField] private Button generateProductNameButton;
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
        [SerializeField] private TextMeshProUGUI profitProductNameText;

        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();
        private readonly GamerTagGenerator _gamerTagGenerator = new GamerTagGenerator();


        [Inject] private AccountService _accountService;


        public void Start()
        {
            generateProductNameButton.OnClickAsObservable().Subscribe(_ => GenerateProductName()).AddTo(this);
            productName.onValueChanged.AsObservable().Subscribe(prodName => profitProductNameText.text = prodName).AddTo(this);
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

        private void GenerateProductName()
        {
            var generatedName = _gamerTagGenerator.Generate();
            productName.text = generatedName;
            profitProductNameText.text = generatedName;
        }
        
        public void SetProductIcon(Sprite icon, Color color)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = color;
            iconImage.sprite = icon;
        }
    }
}