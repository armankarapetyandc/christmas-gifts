using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLoanInfo
{
    public class BusinessLoanInfoPopupController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;

        public BusinessLoanInfoPopupController(PresenterService presenterService, 
            PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
        }
        
        internal void ClosePopup()
        {
            _popupPresenterService.Hide<BusinessLoanInfoPopup>();
        }
    }
}