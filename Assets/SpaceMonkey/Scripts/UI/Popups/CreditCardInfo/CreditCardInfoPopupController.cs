using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCardInfo
{
    public class CreditCardInfoPopupController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;

        public CreditCardInfoPopupController(PresenterService presenterService, PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
        }
        
        internal void ClosePopup()
        {
            _popupPresenterService.Hide<CreditCardInfoPopup>();
        }
    }
}