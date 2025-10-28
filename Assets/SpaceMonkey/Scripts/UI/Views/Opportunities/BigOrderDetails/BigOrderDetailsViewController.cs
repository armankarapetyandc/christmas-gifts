using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BigOrder;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCanceled;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails
{
    public class BigOrderDetailsViewController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService _popupPresenterService;
        private AccountService _accountService;

        public BigOrderDetailsViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,
            PopupPresenterService popupPresenterService, AccountService accountService) : base(presenterService)
        {
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
        }

        internal void OnBack(MainNavigationType type)
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = type
            }).Forget();
        }


        public void OnCancel(MainNavigationType type)
        {
            _accountService.Model.Account.ResetBigOrder();
            _accountService.SaveAsync().Forget();
            PresenterService.Show<BigOrderCanceledView>(new BigOrderCanceledView.Data()
                {
                    Type = type
                }
            ).Forget();
        }
    }
}