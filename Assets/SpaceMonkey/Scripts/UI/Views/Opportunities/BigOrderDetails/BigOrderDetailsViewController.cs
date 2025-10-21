using Cysharp.Threading.Tasks;
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

        public BigOrderDetailsViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,
            PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Map
            }).Forget();
        }

        public void OnInfo()
        {
         
        }
        
        public void OnCancel()
        {
            PresenterService.Show<BigOrderCanceledView>().Forget();
        }

    }
}