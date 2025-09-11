using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.DeleteProduct
{
    public class DeleteProductPopupController:BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;

        public DeleteProductPopupController(PresenterService presenterService,PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
        }
        
        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
    }
}