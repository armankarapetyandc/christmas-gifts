using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductList
{
    public class ProductListController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly NavigationPresenterService _navigationPresenterService;

        public ProductListController(PresenterService presenterService, VisualAssetDatabase visualAssetDatabase,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _visualAssetDatabase = visualAssetDatabase;
            _navigationPresenterService = navigationPresenterService;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        internal SpriteVisualAsset ResolveSpriteVisualAsset(string id)
        {
            return _visualAssetDatabase.GetResourceForAsset<SpriteVisualAsset>(id);
        }

        internal ColorVisualAsset ResolveColorVisualAsset(string id)
        {
            return _visualAssetDatabase.GetResourceForAsset<ColorVisualAsset>(id);
        }
        internal void ProductSelected(Profile.Product product)
        {
            PresenterService.HidePreviousAndShow<ProductView>(new ProductView.Data
            {
                ProductId = product.Id
            }).Forget();
        }

        internal void OnNewProduct()
        {
            PresenterService.HidePreviousAndShow<ProductView>().Forget();
        }
    }
}