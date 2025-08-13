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
        [SerializeField] private Sprite defaultIconHolderSprite;
        [SerializeField] private Sprite defaultIconSprite;

        [SerializeField] private TimeToProduceSlider timeToProductSlider;
        [SerializeField] private MaterialPriceSlider materialPriceSlider;
        [SerializeField] private MaterialPackagingSlider materialPackagingSlider;
        [SerializeField] private ProductPriceSlider productPriceSlider;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI shippingCostText;
        [SerializeField] private TextMeshProUGUI profitProductNameText;

        [SerializeField] private TextMeshProUGUI saveButtonText;
        [SerializeField] private Color selectedSaveTextColor;
        
        [SerializeField] private Button deleteButton;
        [SerializeField] private GameObject ftuePanel;

        internal Observable<Unit> OnIconButtonClicked => iconCreationButton.OnClickAsObservable();

        internal Observable<(bool, ProductData)> OnSaveButtonClicked =>
            saveButton.OnClickAsObservable().Select(_ => (_isExistingProduct, CollectNewProductData()));
        
        internal Observable<ProductData> OnDeleteButtonClicked => deleteButton.OnClickAsObservable().Select(_ => _productData);

        private readonly GamerTagGenerator _gamerTagGenerator = new GamerTagGenerator();
        private ProductData _productData;
        private bool _isExistingProduct = false;
        
        [Inject] private AccountService _accountService;
        [Inject] private ProductIconBuilderConfig _iconBuilderConfig;


        public void Start()
        {
            _productData = new ProductData();
            saveButton.OnClickAsObservable().Subscribe(_ => Reset()).AddTo(this);
            generateProductNameButton.OnClickAsObservable().Subscribe(_ => GenerateProductName()).AddTo(this);
            productName.onValueChanged.AsObservable().Subscribe(prodName => profitProductNameText.text = prodName).AddTo(this);
            productName.onValueChanged.AsObservable().Subscribe(_ => CheckPointsForSaveButton()).AddTo(this);
            SetUpSliders();
            InitCostTexts();
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
            CheckPointsForSaveButton();
        }

        public void ChangeDeleteButtonState(bool state)
        {
            deleteButton.gameObject.SetActive(state);
        }

        public void Reset()
        {
            timeToProductSlider.Reset();
            materialPackagingSlider.Reset();
            materialPriceSlider.Reset();
            productPriceSlider.Reset();
            backgroundImage.gameObject.SetActive(false);
            iconImage.sprite = null;
            backgroundImage.sprite = defaultIconSprite;
            productName.text = string.Empty;
            iconHolder.sprite = defaultIconHolderSprite;
           _productData.Reset();
           _isExistingProduct = false;
        }

        public void SetCurrentData(ProductData productData)
        {
            _isExistingProduct = true;
            _productData = productData;
            ftuePanel.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(true);
            productName.text = productData.Name;
            profitProductNameText.text  = productData.Name;
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.color = productData.BackgroundColor;
            iconImage.sprite = productData.Icon;
            backgroundImage.gameObject.SetActive(true);
            
            SetUpSliders();
            timeToProductSlider.SetInitValue(productData.TimeToProduceIndex);
            materialPriceSlider.SetInitValue(productData.MaterialCost);
            materialPackagingSlider.SetInitValue(productData.PackagingCost);
            
            //InitCostTexts();
        }

        private void SetUpSliders()
        {
            timeToProductSlider.Setup(Constants.TimeToProduct, 1);
            
            bool isPriorityCategory = _accountService.Model.Account.Company.Category == "Cooking";
            float materialAddCoefficient = _accountService.Model.Account.Company.Tags.Select(t => t.MaterialAdd).Sum();
            materialPriceSlider.Setup(materialAddCoefficient, isPriorityCategory);

            float packagingAddCoefficient =
                _accountService.Model.Account.Company.Tags.Select(t => t.PackagingAdd).Sum();
            materialPackagingSlider.Setup(packagingAddCoefficient, isPriorityCategory);


            productPriceSlider.Prepare();
        }

        private void InitCostTexts()
        {
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
        
        private void CheckPointsForSaveButton()
        {
            saveButton.interactable = productName.text.Length > 0 && iconImage.sprite != null;
            saveButtonText.color = saveButton.interactable ? selectedSaveTextColor : Color.white;
            
        }
    }
}