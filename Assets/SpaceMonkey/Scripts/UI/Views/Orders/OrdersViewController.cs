using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.ProductionAlert;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrdersViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly PopupPresenterService _popupPresenterService;
        private readonly GameConfig _gameConfig;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly WeekSimulationContext _weekSimulationContext;
        private ScoresConfigs _scoresConfigs;

        public OrdersViewController(PresenterService presenterService, AccountService accountService,
            PopupPresenterService popupPresenterService,GameConfig gameConfig,
            VisualAssetDatabase visualAssetDatabase,
            WeekSimulationContext weekSimulationContext,ScoresConfigs scoresConfigs) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
            _gameConfig = gameConfig;
            _visualAssetDatabase = visualAssetDatabase;
            _weekSimulationContext = weekSimulationContext;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal CharacterConfig GetCharacter(string id)
        {
            return _gameConfig.Characters.FirstOrDefault(c => c.Id.Equals(id));
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal WeekSimulationV2.Week GetCurrentWeek()
        {
            return _weekSimulationContext.WeekSimulation.CurrentWeek.Value;
        }
        
        internal BigOrderGameData GetBigOrder()
        {
            return _accountService.Model.Account.BigOrderGameData;
        }

        internal Observable<float> GetAvailableProdCapObservable()
        {
            return _weekSimulationContext.WeekSimulation.AvailableProdCap;
        }

        internal float GetAvailableProdCap()
        {
            return _weekSimulationContext.WeekSimulation.AvailableProdCap.CurrentValue;
        }
        
        internal Observable<float> GetMoneyObservable()
        {
            return _weekSimulationContext.WeekSimulation.Money;
        }

        internal async UniTask<bool?> TryShipOrder(WeekSimulationV2.Order order)
        {
            if (order.IsBigOrder || !_weekSimulationContext.WeekSimulation.TryShipOrder(order.Customer.CharacterId))
            {
                var data = new ProductionAlertPopup.Data();
                _popupPresenterService.Show<ProductionAlertPopup>(data).Forget();
                var closeResult = await data.CompletionSource.Task;
                if (closeResult == ProductionAlertPopup.Data.CloseResult.Close)
                {
                    return null;
                }

                return false;
            }

            var score = GetScoreFor("sellProduct") * order.OrderEntries.Sum(o => o.Quantity);
            _weekSimulationContext.WeekSimulation.SellScore += score;
            return true;
        }

        public void FinishWeek()
        {
            _weekSimulationContext.Finish();
        }
        
        internal int GetScoreFor(string key)
        {
            return _scoresConfigs.PeekScoreConfigByKey(key);
        }

        public void AddScore(int score)
        {
            _accountService.Model.Account.Score += score;
        }

        public async UniTaskVoid ForceFinishWeekWithAlert()
        {
            var data = new ProductionAlertPopup.Data();
            _popupPresenterService.Show<ProductionAlertPopup>(data).Forget();
            await data.CompletionSource.Task;
            FinishWeek();
        }
    }
}