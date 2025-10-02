using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto
{
    public class LevelInfoAutoController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;

        public LevelInfoAutoController(PresenterService presenterService,PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
        }
    }
}