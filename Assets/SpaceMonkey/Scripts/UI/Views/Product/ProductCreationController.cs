using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductCreationController : BasePresenterController
    {
        internal AccountService AccountService { get; }
        private readonly NavigationPresenterService _navigationPresenterService;

        public ProductCreationController(PresenterService presenterService, AccountService accountService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            AccountService = accountService;
            _navigationPresenterService = navigationPresenterService;
        }

        public void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>();
        }
    }
}