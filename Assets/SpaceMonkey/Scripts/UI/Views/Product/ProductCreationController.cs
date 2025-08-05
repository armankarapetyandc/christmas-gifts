using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductCreationController : BasePresenterController
    {
        internal AccountService AccountService { get; }

        public ProductCreationController(PresenterService presenterService, AccountService accountService) : base(presenterService)
        {
            AccountService = accountService;
        }
    }
}