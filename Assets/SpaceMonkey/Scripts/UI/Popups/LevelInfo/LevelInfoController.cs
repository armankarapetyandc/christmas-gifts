using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfo
{
    public class LevelInfoController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;

        public LevelInfoController(PresenterService presenterService,PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
        }



        public new  void Close()
        {
            _popupPresenterService.HideLast();
        }

    }
}