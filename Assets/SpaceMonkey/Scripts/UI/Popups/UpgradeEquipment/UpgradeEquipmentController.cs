using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCard;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using static System.String;

namespace SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment
{
    public class UpgradeEquipmentController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly PopupPresenterService _popupPresenterService;
        private readonly ScoresConfigs _scoresConfigs;
        private readonly CreditSimulator _creditSimulator;
        private readonly FireSimulator _fireSimulator;
        private readonly GameConfig _gameConfig;

        public CreditSimulator CreditSimulator => _creditSimulator;
        public GameConfig GameConfig => _gameConfig;
        public UpgradeEquipmentController(PresenterService presenterService, AccountService accountService,
            PopupPresenterService popupPresenterService,ScoresConfigs scoresConfigs, CreditSimulator creditSimulator,
            FireSimulator fireSimulator, GameConfig gameConfig) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _creditSimulator = creditSimulator;
            _fireSimulator = fireSimulator;
            _gameConfig = gameConfig;
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

        // public async UniTask<LevelProdCap> UpgradeLevel(LevelProdCap level, Transform transform)
        // {
        //     _accountService.Model.Account.SetLevel(level);
        //     var score = _scoresConfigs.CalculateScoreConfigByKey($"UpgradeProd{level.Id}");
        //     GetAccount().Score += score;
        //     if (score > 0)
        //     {
        //         XPParticleEffector.SpawnXpParticles(score, new Vector2(Screen.width, Screen.height) * 0.5f, transform)
        //             .Forget();
        //     }
        //
        //     await _accountService.SaveAsync();
        //     await _fireSimulator.RepairFire();
        //
        //     OnClose();
        //     return level;
        // }

        // public async UniTask<bool> MakeCreditCardPurchase(LevelProdCap level)
        // {
        //     var result = await _creditSimulator.MakePurchase(level.ProdCapCost, Empty);
        //     return result;
        // }
        //
        // public async UniTask ShowCreditCardView()
        // {
        //     _creditSimulator.ApplyForCredit().Forget();
        //     var onNextTcs = new UniTaskCompletionSource();
        //     await PresenterService.Show<CreditCardSplashView>(new CreditCardSplashView.Data
        //     {
        //         OnNexTaskCompletionSource = onNextTcs
        //     }, hidePrevious: false);
        //     await onNextTcs.Task;
        //     await PresenterService.Hide();
        // }
        //
        // public async UniTask ShowEquipmentUpgradeSplash()
        // {
        //     PresenterService.Show<EquipmentUpgradeSplashView>().Forget();
        //     await UniTask.WaitUntil(() => PresenterService.GetPresenter<EquipmentUpgradeSplashView>() == false);
        // }
    }
}