using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BigOrder;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderViewController : BasePresenterController

    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService _popupPresenterService;

        public BigOrderViewController(PresenterService presenterService,
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
            _popupPresenterService.Show<BigOrderPopup>().Forget();
        }

        public void OnDecline()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Map
            }).Forget();
        }
        
        public void OnAccept()
        {
            PresenterService.Show<BigOrderCongratulationView>();
        }
    }
}