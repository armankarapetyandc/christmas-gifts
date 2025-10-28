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
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.AllReviewView
{
    public class AllReviewController : BasePresenterController
    {
        private readonly PresenterService _presenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        private readonly AccountService _accountService;
        private readonly WeekSimulationContext _weekSimulationContext;
        private NavigationPresenterService _navigationPresenterService;

        public AllReviewController(PresenterService presenterService, VisualAssetDatabase visualAssetDatabase,GameConfig gameConfig,
            AccountService accountService,WeekSimulationContext weekSimulationContext,NavigationPresenterService navigationPresenterService) : base(
            presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
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

        public  new void Close()
        {
            PresenterService.Hide();
            _navigationPresenterService.HideAll();
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void OnNext()
        {
            _presenterService.Show<ProfitView>().Forget();
        }

        internal float CalculateCompanyOngoingWeekRating()
        {
            return GetAccount().CalculateCompanyRating(_gameConfig.SimulationInfo.MoodRanges,_accountService.Model.Account.WeeksV2);
        }

        internal float CalculateCompanyWeekRating(List<WeekSimulationV2.Week> weeks)
        {
            return Mathf.Floor(GetAccount().CalculateCompanyRating(_gameConfig.SimulationInfo.MoodRanges, weeks)  * 10f) / 10f;
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

        public CharacterConfig GetCharacterConfig(string id)
        {
            return _gameConfig.Characters.FirstOrDefault(config => config.Id == id);
        }



        internal List<WeekSimulationV2.Week> GetWeeks()
        {
            return _accountService.Model.Account.WeeksV2;
        }

        internal IEnumerable<CustomerReviewInfo> GetReviews()
        {
            // var account = GetAccount();
            // return account.Reviews?.Where(info => info.WeekId.Equals(_weekSimulationContext.WeekSimulation.WeekInfo.Id));
            return _accountService.Model.Account.Reviews;
        }
    }
}