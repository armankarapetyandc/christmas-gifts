using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.DeleteProduct;
using SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class ProductController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;

        internal Profile.Product CurrentProduct;
        private ScoresConfigs _scoresConfigs;

        public ProductController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            AccountService accountService,
            VisualAssetDatabase visualAssetDatabase, ScoresConfigs scoresConfigs) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _popupPresenterService = popupPresenterService;
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
            var deleteProductPopupPresenterData = new DeleteProductPopup.Data();
            _popupPresenterService.Show<DeleteProductPopup>(deleteProductPopupPresenterData).Forget();

            var result = await deleteProductPopupPresenterData.GetAwaiter();
            if (result)
            {
                _accountService.Model.Account.DeleteProduct(CurrentProduct.Id);
                await _accountService.SaveAsync();
            }

            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }

        internal async UniTaskVoid SaveProduct(Transform transform)
        {
            if (!CurrentProduct.IsValid)
            {
                CurrentProduct.AssignId();
            }

            _accountService.Model.Account.SetProduct(CurrentProduct);
            var score = _scoresConfigs.CalculateScoreConfigByKey(
                $"addProduct{_accountService.Model.Account.Products.Count}");
            _accountService.Model.Account.Score += score;


            await _accountService.SaveAsync();
            if (score > 0)
            {
                await XPParticleEffector.SpawnXpParticles(score, new Vector2(Screen.width, Screen.height) * 0.5f,
                    transform);
            }

            if (_accountService.Model.Account.Products.Count == 1)
            {
                //show congrats
            }
            else
            {
                PresenterService.HidePreviousAndShow<ProductListView>().Forget();
            }
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
        internal int GetScoreFor(string key)
        {
            return _scoresConfigs.PeekScoreConfigByKey(key);
        }
    }
}