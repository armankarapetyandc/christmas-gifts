using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductList
{
    public class ProductListView : BasePresenterWithController<ProductListController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button newProductButton;
        [SerializeField] private ProductItem productItemPrefab;
        [SerializeField] private RectTransform content;

        [Inject] private AccountService _accountService;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            newProductButton.OnClickAsObservable().Subscribe(_ => Controller.OnNewProduct()).AddTo(this);
            SetupExistingProducts();
            return UniTask.CompletedTask;
        }

        private void SetupExistingProducts()
        {
            var products = _accountService.Model.Account.Products;
            foreach (Profile.Product product in products)
            {
                var item = CreateProduct(product);
                item.Selected.Subscribe(p => Controller.ProductSelected(p)).AddTo(this);
            }
        }

        private ProductItem CreateProduct(Profile.Product product)
        {
            var item = Instantiate(productItemPrefab, content);
            item.Setup(product, Controller.ResolveSpriteVisualAsset(product.IconVisualAssetId),
                Controller.ResolveColorVisualAsset(product.BackgroundColorVisualAssetId));
            return item;
        }

        public override void Dispose()
        {
        }
    }
}