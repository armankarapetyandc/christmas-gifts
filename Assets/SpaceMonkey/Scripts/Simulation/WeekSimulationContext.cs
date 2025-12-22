using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Analytics;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Orders;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss;
using SpaceMonkey.Scripts.UI.Views.Review;
using SpaceMonkey.Scripts.UI.Views.UpgradeCapacity;
using SpaceMonkey.Scripts.UI.Views.WeekReview;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;

namespace SpaceMonkey.Scripts.Simulation
{
    public class WeekSimulationContext : IDisposable
    {
        private readonly WeekSimulationV2.Factory _factory;
        private readonly PresenterService _presenterService;
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly GameConfig gameConfig;

        public WeekSimulationV2 WeekSimulation { get; private set; }

        public WeekSimulationContext(WeekSimulationV2.Factory factory, PresenterService presenterService,AccountService accountService,
            NavigationPresenterService navigationPresenterService,GameConfig gameConfig)
        {
            _factory = factory;
            _presenterService = presenterService;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            this.gameConfig = gameConfig;
        }

        public void Run()
        {
            // WeekSimulation?.Dispose();
            WeekSimulation = _factory.Create();

            WeekSimulation.Initialize();
            WeekSimulation.StartNewWeek(_accountService.Model.Account.Week);

            _navigationPresenterService.HideAll();
            _presenterService.HidePreviousAndShow<OrdersView>().Forget();
        }
        
        public void Dispose()
        {
            // WeekSimulation?.Dispose();
        }

        public void Finish()
        {
            var data = new WeekReviewView.Data();
            var currentScore = _accountService.Model.Account.Score;
            var newScore = currentScore + WeekSimulation.SellScore;
            var levelInfo = gameConfig.LevelInfos.LastOrDefault(info => info.Score <= newScore);
            if (levelInfo != null && levelInfo.Level > _accountService.Model.Account.Level)
            {
                data.LevelIncreased = true;
            }

            WeekSimulation.FinishWeek();
            AnalyticsProvider.SendEvent($"{AnalyticsEvents.WeekFinished}_{_accountService.Model.Account.Week}", new Dictionary<string, string>()
            {
                { "company_name", _accountService.Model.Account.Company.CompanyName },
                { "week", _accountService.Model.Account.Week.ToString() },
                { "orders_count", _accountService.Model.Account.WeeksV2[^1].Orders.Count.ToString() },
                {
                    "fulfilled",
                    _accountService.Model.Account.WeeksV2[^1].Orders.All(order => order.WasFulfilled).ToString()
                },
                { "level", _accountService.Model.Account.Level.ToString() },
                { "score", _accountService.Model.Account.Score.ToString() },
                { "cash", _accountService.Model.Account.Money.ToString() }
            });
            Finished(data.LevelIncreased);
            // _presenterService.HidePreviousAndShow<WeekReviewView>(data).Forget();
        }
        private IEnumerable<CustomerReviewInfo> GetReviews()
        {
            return _accountService.Model.Account.Reviews?.Where(info =>
                info.WeekId.Equals(WeekSimulation.CurrentWeek.Value.Id));
        }
        private void Finished(bool? levelIncreased)
        {
            if (levelIncreased != null && levelIncreased.Value)
            {
                _presenterService.HidePreviousAndShow<WeekEndRewardView>().Forget();
                return;
            }
            
            if (GetReviews()?.Count() > 0)
            {
                _presenterService.HidePreviousAndShow<ReviewView>(new ReviewView.Data()
                {
                    IsWeekEnd = true
                }).Forget();
                return;
            }
            
            _presenterService.Show<ProfitView>(new ProfitView.Data
            {
                UseSimulation = true,
                IsWeekEnd = true
            }).Forget();
        }
    }
}