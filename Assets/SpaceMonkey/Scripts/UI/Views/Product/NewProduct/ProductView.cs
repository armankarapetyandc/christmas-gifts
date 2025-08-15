using System.Linq;
using Cysharp.Threading.Tasks;
using NameGenerator.Generators;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Extensions;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class ProductView : BasePresenterWithController<ProductController>
    {
        public class Data : IPresenterData
        {
            public Profile.Product? SelectedProduct { get; set; }
        }

        [SerializeField] private TMP_InputField productNameInputField;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private Button generateProductNameButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button backButton;

        [SerializeField] private TimeToProduceSlider timeToProductSlider;
        [SerializeField] private MaterialPriceSlider materialPriceSlider;
        [SerializeField] private MaterialPackagingSlider materialPackagingSlider;
        [SerializeField] private ProductPriceSlider productPriceSlider;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI shippingCostText;
        [SerializeField] private TextMeshProUGUI profitProductNameText;

        [SerializeField] private Button deleteButton;
        [SerializeField] private ColorVisualAsset defaultIconColorVisualAsset;

        public override async UniTask Initialize(IPresenterData data = null)
        {
            var presenterData = data as Data;
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.SaveProduct()).AddTo(this);
            iconComponent.OnClick.Subscribe(_ => Controller.SelectProductIcon()).AddTo(this);
            generateProductNameButton.OnClickAsObservable().Subscribe(_ => GenerateProductName()).AddTo(this);
            productNameInputField.onValueChanged.AsObservable().Subscribe(p => profitProductNameText.text = p)
                .AddTo(this);
            SetupDefaults(presenterData?.SelectedProduct);
            ListenProductChanges();
        }

        private void SetupDefaults(Profile.Product? product)
        {
            Controller.SetProduct(product);
            deleteButton.gameObject.SetActive(product != null);
            productNameInputField.text = Controller.CurrentProduct.Name;
            iconComponent.SetIcon(
                Controller.ResolveVisualAsset<SpriteVisualAsset>(Controller.CurrentProduct.IconVisualAssetId));
            iconComponent.SetColor(
                Controller.ResolveVisualAsset<ColorVisualAsset>(Controller.CurrentProduct.BackgroundColorVisualAssetId));
            ConfigureSliders();
        }

        private void ConfigureSliders()
        {
            timeToProductSlider.Setup(Constants.TimeToProduct, 1);

            var account = Controller.GetAccount();

            bool isPriorityCategory = Constants.PriorityCategories.Contains(account.Company.Category);
            float materialAddCoefficient = account.Company.Tags.Select(t => t.MaterialAdd).Sum();
            materialPriceSlider.Setup(materialAddCoefficient, isPriorityCategory);

            float packagingAddCoefficient = account.Company.Tags.Select(t => t.PackagingAdd).Sum();
            materialPackagingSlider.Setup(packagingAddCoefficient, isPriorityCategory);

            productPriceSlider.Prepare();
        }

        private void ListenProductChanges()
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
                Controller.CurrentProduct.TotalCost = totalCost;
                Controller.CurrentProduct.TtpCost = ttpCost;
                Controller.CurrentProduct.ShippingCost = shippingCost;
                productPriceSlider.UpdateRange(totalCost, productMaxPrice);
            }).AddTo(this);
        }


        private void GenerateProductName()
        {
            GamerTagGenerator gamerTagGenerator = new GamerTagGenerator();
            var generatedName = gamerTagGenerator.Generate();
            productNameInputField.text = generatedName;
            profitProductNameText.text = generatedName;
        }

        public override void Dispose()
        {
        }
    }
}