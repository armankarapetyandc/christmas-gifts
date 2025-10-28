using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.BigOrder
{
    public class BigOrderPopupController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;
        private NavigationPresenterService _navigationPresenterService;

        public BigOrderPopupController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }

        public void OpenBigOrderView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<BigOrderView>(new BigOrderView.Data()
            {
                Type = MainNavigationType.Map
            }).Forget();
        }
    }
}