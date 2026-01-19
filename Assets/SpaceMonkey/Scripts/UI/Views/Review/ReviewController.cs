using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Review
{
    public class ReviewController : BasePresenterController
    {
        private readonly PresenterService _presenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        private readonly AccountService _accountService;
        private readonly WeekSimulationContext _weekSimulationContext;

        public ReviewController(PresenterService presenterService, VisualAssetDatabase visualAssetDatabase,GameConfig gameConfig,
            AccountService accountService,WeekSimulationContext weekSimulationContext) : base(
            presenterService)
        {
            _presenterService = presenterService;
            _visualAssetDatabase = visualAssetDatabase;
            _gameConfig = gameConfig;
            _accountService = accountService;
            _weekSimulationContext = weekSimulationContext;
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }
        
        internal WeekSimulationV2.Week GetSimulationWeek()
        {
            return _weekSimulationContext.WeekSimulation.CurrentWeek.Value;
        }
        
        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void OnNext(bool isWeekEnd)
        {
            _presenterService.Show<ProfitView>(new ProfitView.Data
            {
                UseSimulation = true,
                IsWeekEnd = isWeekEnd
            }).Forget();
        }

        internal float CalculateCompanyRating()
        {
            return GetAccount().CalculateCompanyRating(_gameConfig.SimulationInfo.MoodRanges,_accountService.Model.Account.WeeksV2);
        }

        internal float GetRatingByCustomerMood(int value)
        {
            var moodRanges = _gameConfig.SimulationInfo.MoodRanges;
            for (int i = 0; i < moodRanges.Length; i++)
            {
                if (value >= moodRanges[i].Min && value<= moodRanges[i].Max)
                {
                    return i + 1f;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(value), "Mood value must be between 1 and 100.");
        }

        internal IEnumerable<CustomerReviewInfo> GetReviews()
        {
            var account = GetAccount();
            return account.Reviews?.Where(info =>
                    info.WeekId.Equals(_weekSimulationContext.WeekSimulation.CurrentWeek.Value.Id))
                .Distinct(new CustomerReviewComparer());
        }

        public CharacterConfig GetCharacterConfig(string customerCharacterId)
        {
            return _gameConfig.Characters.FirstOrDefault(c => c.Id.Equals(customerCharacterId));
        }
    }
}