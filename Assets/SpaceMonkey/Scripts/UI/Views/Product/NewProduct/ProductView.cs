using System.Linq;
using Cysharp.Threading.Tasks;
using NameGenerator.Generators;
using R3;
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
            public string ProductId { get; set; }
        }

        [SerializeField] private TMP_InputField productNameInputField;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private Button generateProductNameButton;
        [SerializeField] private Button saveButton;
    

        [SerializeField] private TimeToProduceSlider timeToProductSlider;
        [SerializeField] private MaterialPriceSlider materialPriceSlider;
        [SerializeField] private MaterialPackagingSlider materialPackagingSlider;
        [SerializeField] private ProductPriceSlider productPriceSlider;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI shippingCostText;
        [SerializeField] private TextMeshProUGUI profitProductNameText;
        
        [SerializeField] private Button deleteButton;

        private Profile.Product _product;

        public override async UniTask Initialize(IPresenterData data = null)
        {
            var presenterData = data as Data;
            _product = !string.IsNullOrEmpty(presenterData?.ProductId)
                ? Controller.GetAccount().Products?.Find(p => p.Id.Equals(presenterData.ProductId))
                : null;
            saveButton.OnClickAsObservable().Subscribe(_ => Controller.SaveProduct()).AddTo(this);
            generateProductNameButton.OnClickAsObservable().Subscribe(_ => GenerateProductName()).AddTo(this);
            productNameInputField.onValueChanged.AsObservable().Subscribe(p => profitProductNameText.text = p)
                .AddTo(this);
            ConfigureSliders();

            if (_product != null)
            {
                ListenProductChanges();
                await UniTask.Yield();
                timeToProductSlider.Set(_product.TimeToProduceIndex);
                materialPriceSlider.Set(_product.MaterialCost);
                materialPackagingSlider.Set(_product.PackagingCost);
            }
            else
            {
                _product = Profile.Product.CreateEmpty();
                ListenProductChanges();
            }
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
                _product.TotalCost = totalCost;
                _product.TtpCost = ttpCost;
                _product.ShippingCost = shippingCost;
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