using System.Linq;
using Cysharp.Threading.Tasks;
using NameGenerator.Generators;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Extensions;
using SpaceMonkey.Scripts.Utilities.Validation;
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

        [SerializeField] private RectTransform propsHolder;
        [SerializeField] private TMP_InputField productNameInputField;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private Button generateProductNameButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button backButton;

        [SerializeField] private TimeToProduceSlider timeToProductSlider;
        [SerializeField] private MaterialPriceSlider materialPriceSlider;
        [SerializeField] private MaterialPackagingSlider materialPackagingSlider;
        [SerializeField] private ProductPriceSlider productPriceSlider;
        [SerializeField] private RectTransform parametersContainer;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI shippingCostText;
        [SerializeField] private TextMeshProUGUI profitProductNameText;
        [SerializeField] private HotItemSubView hotItemSubView;

        [SerializeField] private Button deleteButton;
        public IconComponent IconComponent => iconComponent;
        public TMP_InputField InputField => productNameInputField;

        public HotItemSubView HotItemSubView =>  hotItemSubView;
        public RectTransform ParametersContainer => parametersContainer;
        public RectTransform PropsHolder => propsHolder;
        public Observable<Unit> ParametersChanged =>
            Observable.Merge(
                timeToProductSlider.CurrentValue.Skip(1).AsUnitObservable(),
                materialPriceSlider.CurrentValue.Skip(1).AsUnitObservable(),
                materialPackagingSlider.CurrentValue.Skip(1).AsUnitObservable(),
                productPriceSlider.CurrentValue.Skip(1).AsUnitObservable()
            );
        
        public Observable<Unit> AllParametersEdited =>
            Observable.CombineLatest(
                    timeToProductSlider.CurrentValue
                        .Skip(1)
                        .Select(_ => true)
                        .Scan(false, (_, __) => true),

                    materialPriceSlider.CurrentValue
                        .Skip(1)
                        .Select(_ => true)
                        .Scan(false, (_, __) => true),

                    materialPackagingSlider.CurrentValue
                        .Skip(1)
                        .Select(_ => true)
                        .Scan(false, (_, __) => true),

                    productPriceSlider.CurrentValue
                        .Skip(1)
                        .Select(_ => true)
                        .Scan(false, (_, __) => true),

                    (t, m, p, pr) => t && m && p && pr
                )
                .DistinctUntilChanged()
                .Where(allEdited => allEdited)
                .AsUnitObservable();
        public Button SaveButton => saveButton;
        public override async UniTask Initialize(IPresenterData data = null)
        {
            var presenterData = data as Data;
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.SaveProduct(transform).Forget()).AddTo(this);
            deleteButton.OnClickAsObservable().Subscribe(_ => Controller.DeleteProduct().Forget()).AddTo(this);
            iconComponent.OnClick.Subscribe(_ => Controller.SelectProductIcon()).AddTo(this);
            generateProductNameButton.OnClickAsObservable().Subscribe(_ => GenerateProductName()).AddTo(this);
            productNameInputField.onValueChanged.AsObservable().Subscribe(p =>
                {
                    profitProductNameText.text = p;
                    Controller.CurrentProduct.Name = p;
                })
                .AddTo(this);
            SetupDefaults(presenterData?.SelectedProduct);
            ListenProductChanges();
            Validator
                .Validate(
                    iconComponent.Fulfilled, productNameInputField.NotEmpty()
                )
                .BindButton(saveButton)
                .AddTo(this);
        }

        public void HideBaseHolder()
        {
            presenterHolder.gameObject.SetActive(false);
        }

        private void SetupDefaults(Profile.Product? product)
        {
            Controller.SetProduct(product);
            deleteButton.gameObject.SetActive(product?.IsValid ?? false);
            productNameInputField.text = Controller.CurrentProduct.Name;
            iconComponent.SetIcon(
                Controller.ResolveVisualAsset<SpriteVisualAsset>(Controller.CurrentProduct.IconVisualAssetId));
            iconComponent.SetColor(
                Controller.ResolveVisualAsset<ColorVisualAsset>(Controller.CurrentProduct
                    .BackgroundColorVisualAssetId));
            ConfigureSliders();
        }

        private void ConfigureSliders()
        {
            timeToProductSlider.Setup(Constants.TimeToProduct, Controller.CurrentProduct.TimeToProduceIndex);

            var account = Controller.GetAccount();

            bool isPriorityCategory = Constants.PriorityCategories.Contains(account.Company.Category);
            float materialAddCoefficient = account.Company.Tags.Select(t => t.MaterialAdd).Sum();
            materialPriceSlider.Setup(materialAddCoefficient, isPriorityCategory,
                Controller.CurrentProduct.MaterialPrice);


            float packagingAddCoefficient = account.Company.Tags.Select(t => t.PackagingAdd).Sum();
            materialPackagingSlider.Setup(packagingAddCoefficient, isPriorityCategory,
                Controller.CurrentProduct.MaterialPackagingPrice);

            // productPriceSlider.Prepare();
            productPriceSlider.Prepare(Controller.CurrentProduct.MinProductPrice,
                Controller.CurrentProduct.MaxProductPrice, Controller.CurrentProduct.ProductPrice);
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
                // Controller.CurrentProduct.TotalCost = totalCost;
                // Controller.CurrentProduct.TtpCost = ttpCost;
                // Controller.CurrentProduct.ShippingCost = shippingCost;
                Controller.CurrentProduct.ShippingCost = shippingCost;
                Controller.CurrentProduct.TimeToProduceIndex =
                    Mathf.RoundToInt(timeToProductSlider.CurrentValue.CurrentValue);
                Controller.CurrentProduct.MaterialPrice = materialPriceSlider.CurrentValue.CurrentValue;
                Controller.CurrentProduct.MaterialPackagingPrice =
                    materialPackagingSlider.CurrentValue.CurrentValue;
                productPriceSlider.UpdateRange(totalCost, productMaxPrice);
                Controller.CurrentProduct.MinProductPrice = totalCost;
                Controller.CurrentProduct.MaxProductPrice = productMaxPrice;
                Controller.CurrentProduct.ProdCapCost = CalculateProdCapCost();
            }).AddTo(this);

            productPriceSlider.CurrentValue.DistinctUntilChanged()
                .Subscribe(value =>
                {
                    Controller.CurrentProduct.ProductPrice = value;
                    Controller.CurrentProduct.Profit = productPriceSlider.CalculateCurrentProfit();
                }).AddTo(this);
        }

        private float CalculateProdCapCost()
        {
            var timeProdCapAdd = Constants.TimeToProduct.Count - timeToProductSlider.CurrentValue.CurrentValue;
            var matProdCapAdd = materialPriceSlider.GetSliderValue() switch
            {
                >= 66f => 2f,
                > 33f => 1f,
                _ => 0f
            };
            var packProdCapAdd = materialPackagingSlider.GetSliderValue() switch
            {
                >= 66f => 2f,
                > 33f => 1f,
                _ => 0.6f
            };
            return (timeProdCapAdd + matProdCapAdd + packProdCapAdd) / 3f;
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