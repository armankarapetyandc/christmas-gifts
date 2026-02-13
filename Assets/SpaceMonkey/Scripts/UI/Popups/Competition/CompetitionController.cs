using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Map;
using SpaceMonkey.Scripts.UI.Views.Product.NewProduct;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Popups.Competition
{
    public class CompetitionController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;
        private GameConfig _gameConfig;
        private AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly NavigationPresenterService _navigationPresenterService;

        public CompetitionController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService, AccountService accountService, 
            VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
        }


        public Product GetCompetitionProduct()
        {
            return _accountService.GetCompetitionProduct();
        }
        public Product GetSavedProduct()
        {
            return _accountService.GetSavedProduct();

        }
        
        public void HideMapIcon()
        {
            var mapView = PresenterService.GetPresenter<MapView>();
            mapView.HidePlace(PlaceType.TreatyBird);
        }


        public void ShowProductsView(Profile.Product product)
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductView>(new ProductView.Data
            {
                SelectedProduct = product
            }).Forget();
            Close();
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
        
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }
    }
}