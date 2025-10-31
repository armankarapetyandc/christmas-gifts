using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss;
using SpaceMonkey.Scripts.UI.Views.Review;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.UpgradeCapacity
{
    public class WeekEndRewardController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private GameConfig _gameConfig;
        private WeekSimulationContext _weekSimulationContext;

        public int Score => (int)_accountService.Model.Account.Score;
        public int Level => _accountService.Model.Account.Level;
        public float SellScore => _weekSimulationContext.WeekSimulation.SellScore;

        public WeekEndRewardController(PresenterService presenterService, AccountService accountService,
            GameConfig gameConfig,WeekSimulationContext weekSimulationContext) : base(presenterService)
        {
            _weekSimulationContext = weekSimulationContext;
            _accountService = accountService;
            _gameConfig = gameConfig;
        }

        public int GetLevel(float score)
        {
            LevelInfo levelInfo = _gameConfig.LevelInfos.Where(info => info.Score <= score)
                .DefaultIfEmpty(_gameConfig.LevelInfos[0]).Max();
            return levelInfo.Level;
        }

        public LevelInfo GetLevelInfoByLevel(int level)
        {
           return _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
        }

        public LevelInfo GetLeveInfoData(int level)
        {
            LevelInfo levelInfo = _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
            return levelInfo ?? _gameConfig.LevelInfos[^1];
        }
        internal void OnNext()
        {
            if (GetReviews()?.Count() > 0)
            {
                PresenterService.HidePreviousAndShow<ReviewView>().Forget();   
                return;
            }
            
            PresenterService.Show<ProfitView>(new ProfitView.Data
            {
                UseSimulation = true
            }).Forget();
        }

        private IEnumerable<CustomerReviewInfo> GetReviews()
        {
            var account = GetAccount();
            return account.Reviews?.Where(info => info.WeekId.Equals(_weekSimulationContext.WeekSimulation.CurrentWeek.Value.Id));
        }

        private Account GetAccount()
        {
            return _accountService.Model.Account;
        }
    }
}