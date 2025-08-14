using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class ProductController:BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;

        public ProductController(PresenterService presenterService,AccountService accountService,VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
        }

        internal SpriteVisualAsset ResolveSpriteVisualAsset(string id)
        {
            return _visualAssetDatabase.GetResourceForAsset<SpriteVisualAsset>(id);
        }
        
        internal ColorVisualAsset ResolveColorVisualAsset(string id)
        {
            return _visualAssetDatabase.GetResourceForAsset<ColorVisualAsset>(id);
        }
        
        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal void SaveProduct()
        {
            throw new System.NotImplementedException();
        }
    }
}