using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
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

        public CompetitionController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            AccountService accountService) :
            base(presenterService)
        {
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
        }


        public Product GetCompetitionProduct()
        {
           return _accountService.Model.Account.Products.FirstOrDefault(product =>
                product.Id == PlayerPrefs.GetString("productId"));
        }

        public void ShowProductsView()
        {
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
    }
}