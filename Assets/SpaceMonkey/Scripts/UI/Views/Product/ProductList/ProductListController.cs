using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
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
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly NavigationPresenterService _navigationPresenterService;

        public ProductListController(PresenterService presenterService, AccountService accountService,VisualAssetDatabase visualAssetDatabase,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
            _navigationPresenterService = navigationPresenterService;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }
        
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }
        
        internal void OnProduct(Profile.Product? product)
        {
            PresenterService.HidePreviousAndShow<ProductView>(new ProductView.Data
            {
                SelectedProduct = product
            }).Forget();
        }
    }
}