using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BigOrder;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.DeleteProduct;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderViewController : BasePresenterController

    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService _popupPresenterService;
        private AccountService _accountService;

        public BigOrderViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,
            PopupPresenterService popupPresenterService,AccountService accountService) : base(presenterService)
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



        public void OnDecline(MainNavigationType type)
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = type
            }).Forget();
        }
        
        public async Task OnAccept(MainNavigationType type)
        {
            _accountService.Model.Account.CreateBigOrder();
            await _accountService.SaveAsync();
            PresenterService.Show<BigOrderCongratulationView>(new BigOrderCongratulationView.Data()
            {
                Type = type
            });
        }
    }
}