using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment
{
    public class UpgradeEquipmentController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly PopupPresenterService _popupPresenterService;
        private ScoresConfigs _scoresConfigs;

        public UpgradeEquipmentController(PresenterService presenterService, AccountService accountService,
            PopupPresenterService popupPresenterService,ScoresConfigs scoresConfigs) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
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

        public async Task<LevelProdCap> UpgradeLevel(LevelProdCap level)
        {
            _accountService.Model.Account.SetLevel(level);
            GetAccount().Score += _scoresConfigs.CalculateScoreConfigByKey($"UpgradeProd{level.Id}");
            _accountService.SaveAsync();
            OnClose();
            return level;
        }

        public void UseCredit()
        {
            PresenterService.Show<CreditCardStatementView>(new CreditCardStatementView.Data
            {
                OnClose = () => { PresenterService.Show<ProductionCapacityView>(); }
            });
            OnClose();
        }
    }
}