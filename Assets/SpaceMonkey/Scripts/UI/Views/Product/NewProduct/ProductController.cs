using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class ProductController:BasePresenterController
    {
        private readonly AccountService _accountService;

        public ProductController(PresenterService presenterService,AccountService accountService) : base(presenterService)
        {
            _accountService = accountService;
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