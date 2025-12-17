using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
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
        private readonly NavigationPresenterService _navigationPresenterService;

        public CompetitionController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService, AccountService accountService) :
            base(presenterService)
        {
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
        }


        public Product GetCompetitionProduct()
        {
           return _accountService.Model.Account.Products.FirstOrDefault(product =>
                product.Id == PlayerPrefs.GetString("productId"));
        }

        public void ShowProductsView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
            Close();
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
    }
}