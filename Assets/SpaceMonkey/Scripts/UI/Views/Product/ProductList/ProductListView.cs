using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductList
{
    public class ProductListView : BasePresenterWithController<ProductListController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button newProductButton;
        [SerializeField] private ProductItem productItemPrefab;
        [SerializeField] private RectTransform content;

        private readonly ObservableList<ProductItem> _items = new ObservableList<ProductItem>();

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            newProductButton.OnClickAsObservable().Subscribe(_ => Controller.OnProduct(null)).AddTo(this);
            _items
                .ObserveAdd()
                .Select(e => e.Value.Selected)
                .Merge()
                .Subscribe(p => Controller.OnProduct(p))
                .AddTo(this);

            SetupDefaults();
            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            foreach (Profile.Product product in account.Products)
            {
                var item = CreateProduct(product);
                _items.Add(item);
            }
        }

        private ProductItem CreateProduct(Profile.Product product)
        {
            var item = Instantiate(productItemPrefab, content);
            item.Setup(product);
            var iconVisualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(product.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(product.BackgroundColorVisualAssetId);
            item.SetVisual(iconVisualAsset, colorVisualAsset);
            return item;
        }

        public override void Dispose()
        {
            _items.Clear();
        }
    }
}