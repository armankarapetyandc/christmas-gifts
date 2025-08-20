using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment
{
    public class UpgradeEquipmentController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly PopupPresenterService _popupPresenterService;

        public UpgradeEquipmentController(PresenterService presenterService, AccountService accountService,
            PopupPresenterService popupPresenterService) : base(presenterService)
        {
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
        }

        internal void OnClose()
        {
            _popupPresenterService.Hide<UpgradeEquipmentPopup>();
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }
    }
}