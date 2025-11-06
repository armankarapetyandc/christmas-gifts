using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Orders;
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
            _presenterService.HidePreviousAndShow<WeekReviewView>(data).Forget();
        }
    }
}