using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class ProductController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;

        internal Profile.Product CurrentProduct;

        public ProductController(PresenterService presenterService, AccountService accountService,
            VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
        }


        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal void OnBack()
        {
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }
        public async UniTaskVoid DeleteProduct()
        {
            _accountService.Model.Account.DeleteProduct(CurrentProduct.Id);
            await _accountService.SaveAsync();
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }
        internal async UniTaskVoid SaveProduct()
        {
            CurrentProduct.AssignId();
            _accountService.Model.Account.SetProduct(CurrentProduct);
            await _accountService.SaveAsync();
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }

        internal void SetProduct(Profile.Product? product)
        {
            CurrentProduct = product ?? Profile.Product.CreateEmpty();
        }

        internal void SelectProductIcon()
        {
            PresenterService.HidePreviousAndShow<ProductIconBuilderView>(new ProductIconBuilderView.Data
            {
                Product = CurrentProduct
            }).Forget();
        }

       
    }
}