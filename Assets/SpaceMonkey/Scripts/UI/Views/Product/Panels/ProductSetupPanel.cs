using System;
using System.Linq;
using NameGenerator.Generators;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Product;
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
        [SerializeField] private Image iconHolder;
        [SerializeField] private Sprite defaultIconSpace;

        [SerializeField] private TimeToProduceSlider timeToProductSlider;
        [SerializeField] private MaterialPriceSlider materialPriceSlider;
        [SerializeField] private MaterialPackagingSlider materialPackagingSlider;
        [SerializeField] private ProductPriceSlider productPriceSlider;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI shippingCostText;
        [SerializeField] private TextMeshProUGUI profitProductNameText;
        
        [SerializeField] private Button deleteButton;
        [SerializeField] private GameObject ftuePanel;

        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();

        internal Observable<ProductData> OnSaveButtonClicked =>
            saveButton.OnClickAsObservable().Select(_ => CollectNewProductData());
        
        internal Observable<ProductData> OnDeleteButtonClicked => deleteButton.OnClickAsObservable().Select(_ => _productData);

        private readonly GamerTagGenerator _gamerTagGenerator = new GamerTagGenerator();
        private ProductData _productData;


        [Inject] private AccountService _accountService;
        [Inject] private ProductIconBuilderConfig _iconBuilderConfig;


        public void Start()
        {
            _productData = new ProductData();
            saveButton.OnClickAsObservable().Subscribe(_ => Reset()).AddTo(this);
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
                _productData.TotalCost = totalCost;
                _productData.TtpCost = ttpCost;
                _productData.ShippingCost = shippingCost;
                productPriceSlider.UpdateRange(totalCost, productMaxPrice);
            }).AddTo(this);
        }
        
        private ProductData CollectNewProductData()
        {
            _productData.Name = productName.text;
            _productData.Icon = iconImage.sprite;
            _productData.BackgroundColor = backgroundImage.color;
            _productData.Price = productPriceSlider.CurrentValue.CurrentValue;
            _productData.Profit = productPriceSlider.Profit;
            
            _productData.TimeToProduceIndex = timeToProductSlider.SliderValue;
            _productData.MaterialCost = materialPriceSlider.SliderValue;
            _productData.PackagingCost = materialPackagingSlider.SliderValue;
            return _productData;
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

        public void ChangeDeleteButtonState(bool state)
        {
            deleteButton.gameObject.SetActive(state);
        }

        private void Reset()
        {
            timeToProductSlider.Reset();
            materialPackagingSlider.Reset();
            materialPriceSlider.Reset();
            productPriceSlider.Reset();
            backgroundImage.gameObject.SetActive(false);
            iconImage.sprite = null;
            backgroundImage.sprite = null;
            backgroundImage.color = Color.white;
            productName.text = string.Empty;
            iconHolder.sprite = defaultIconSpace;
           _productData.Reset();
        }

        public void SetCurrentData(ProductData productData)
        {
            ftuePanel.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(true);
            productName.text = productData.Name;
            profitProductNameText.text  = productData.Name;
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = productData.BackgroundColor;
            iconImage.sprite = productData.Icon;
            backgroundImage.gameObject.SetActive(true);
            
            timeToProductSlider.Setup(Constants.TimeToProduct, 1);
            
            bool isPriorityCategory = _accountService.Model.Account.Company.Category == "Cooking";
            float materialAddCoefficient = _accountService.Model.Account.Company.Tags.Select(t => t.MaterialAdd).Sum();
            materialPriceSlider.Setup(materialAddCoefficient, isPriorityCategory);
            
            float packagingAddCoefficient =
                _accountService.Model.Account.Company.Tags.Select(t => t.PackagingAdd).Sum();
            materialPackagingSlider.Setup(packagingAddCoefficient, isPriorityCategory);
            
            productPriceSlider.Prepare();
            
            timeToProductSlider.SetInitValue(productData.TimeToProduceIndex);
            materialPriceSlider.SetInitValue(productData.MaterialCost);
            materialPackagingSlider.SetInitValue(productData.PackagingCost);
        }
        
    }
}