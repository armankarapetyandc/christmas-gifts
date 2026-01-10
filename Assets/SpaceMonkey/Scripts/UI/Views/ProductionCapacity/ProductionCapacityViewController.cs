using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment;
using SpaceMonkey.Scripts.UI.Views.LevelUpdate;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash;
using SpaceMonkey.Scripts.UI.Views.Opportunities.FireRepairSplash;
using TMPro;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityViewController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly PopupPresenterService  _popupPresenterService;
        private readonly FireSimulator _fireSimulator;
        private readonly ScoresConfigs _scoresConfigs;
        private readonly CreditSimulator _creditSimulator;
        public CreditSimulator CreditSimulator => _creditSimulator;

        public ProductionCapacityViewController(PresenterService presenterService,
            VisualAssetDatabase visualAssetDatabase, GameConfig gameConfig, AccountService accountService,
            NavigationPresenterService navigationPresenterService, PopupPresenterService popupPresenterService,
            FireSimulator fireSimulator, ScoresConfigs scoresConfigs, CreditSimulator creditSimulator) : base(presenterService)
        {
            _visualAssetDatabase = visualAssetDatabase;
            _gameConfig = gameConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            _popupPresenterService = popupPresenterService;
            _fireSimulator = fireSimulator;
            _scoresConfigs = scoresConfigs;
            _creditSimulator = creditSimulator;
        }

        internal async UniTask<bool> OpenUpgradeEquipmentPopup(LevelProdCap level)
        {
            var tcs = new UniTaskCompletionSource<bool>();
            _popupPresenterService.Show<UpgradeEquipmentPopup>(new UpgradeEquipmentPopup.Data
            {
                UpgradeLevelProdCap = level,
                Result = tcs
            });
            return await tcs.Task;;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        public int GetProdCupDifference()
        {
            int prodCup = _accountService.Model.Account.GetProductionCapacity();
            float ratio = _accountService.Model.Account.GetCapacityReductionPercent();
            int oldProdCup = (int)((prodCup / ratio) - prodCup);
            return oldProdCup;
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        public void UpgradeLevel(int levelNumber, int score, Sprite levelSprite, TMP_FontAsset levelFontAsset)
        {
            PresenterService.Show<LevelUpdateView>(new LevelUpdateView.Data
            {
                LevelNumber = levelNumber,
                LevelSprite = levelSprite,
                IsUpgraded = true,
                FontAsset = levelFontAsset, 
                Score = score 
            });
        }
        
        public async UniTask<(LevelProdCap level, int score)> UpgradeLevel(LevelProdCap level, Transform transform)
        {
            _accountService.Model.Account.SetLevel(level);
            var score = _scoresConfigs.CalculateScoreConfigByKey($"UpgradeProd{level.Id}");
            GetAccount().Score += score;
            
            await _accountService.SaveAsync();
            await _fireSimulator.RepairFire();
            
            return (level, score);
        }
        
        public float GetFireRepairCost()
        {
            return _fireSimulator.GetRepairCost();
        }
        
        public UniTask<bool> RepairFireDamage(float cost)
        {
            return _fireSimulator.RepairFire(cost);
        }
        
        public void ShowFireRepairSplash(LevelProdCap level)
        {
            PresenterService.Show<FireRepairSplashView>(new FireRepairSplashView.Data
            {
                Level = level
            }).Forget();
        }
        
        public async UniTask<bool> MakeCreditCardPurchase(LevelProdCap level)
        {
            var result = await _creditSimulator.MakePurchase(level.ProdCapCost, String.Empty);
            return result;
        }
        
        public async UniTask ShowCreditCardView()
        {
            _creditSimulator.ApplyForCredit().Forget();
            var onNextTcs = new UniTaskCompletionSource();
            await PresenterService.Show<CreditCardSplashView>(new CreditCardSplashView.Data
            {
                OnNexTaskCompletionSource = onNextTcs
            }, hidePrevious: false);
            await onNextTcs.Task;
            await PresenterService.Hide();
        }
    }
}