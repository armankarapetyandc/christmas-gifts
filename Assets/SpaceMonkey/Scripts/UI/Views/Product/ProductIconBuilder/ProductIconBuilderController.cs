using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder
{
    public class ProductIconBuilderController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;

        internal Profile.Product Product;


        public ProductIconBuilderController(PresenterService presenterService, AccountService accountService,
            VisualAssetDatabase visualAssetDatabase)
            : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal ColorVisualAsset GetDefaultColorVisualAssetInSequence()
        {
            var account = GetAccount();
            
            var colorVisualAssets =
                ResolveVisualAssets<OrderedColorVisualAsset>(asset => asset.Type.HasFlag(VisualAssetType.Product))
                    .OrderBy(asset => asset.Order)
                    .Cast<ColorVisualAsset>()
                    .ToList();
            var safeIndex = account.Products.Count % colorVisualAssets.Count;
            return colorVisualAssets[safeIndex];
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void ReturnProductView(Profile.Product product)
        {
            PresenterService.HidePreviousAndShow<ProductView>(new ProductView.Data
            {
                SelectedProduct = product
            }).Forget();
        }
    }
}