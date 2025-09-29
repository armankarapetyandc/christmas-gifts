using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
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
        private readonly ScoresConfigs _scoresConfigs;
        private readonly CreditSimulator _creditSimulator;

        public CreditSimulator CreditSimulator => _creditSimulator;
        public UpgradeEquipmentController(PresenterService presenterService, AccountService accountService,
            PopupPresenterService popupPresenterService,ScoresConfigs scoresConfigs, CreditSimulator creditSimulator) 
            : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _creditSimulator = creditSimulator;
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
        
        public bool MakeCreditCardPurchase(LevelProdCap level)
        {
            return _creditSimulator.MakePurchase(level.ProdCapCost);
        }
        
        public void ShowCreditCardView()
        {
            PresenterService.Show<CreditCardView>().Forget();
        }
    }
}