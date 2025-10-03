using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCard
{
    public class CreditCardPopupController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly NavigationPresenterService _navigationPresenterService;
        public CreditCardPopupController(PresenterService presenterService,PopupPresenterService popupPresenterService,NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
        
        public void RedirectToCreditCardStatement()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<CreditCardView>().Forget();
        }
    }
}