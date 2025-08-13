using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Asset;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Asset.Product;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
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

        internal void ProductSelected(Profile.Product product)
        {
            throw new System.NotImplementedException();
        }

        internal void OnNewProduct()
        {
            throw new System.NotImplementedException();
        }
    }
}