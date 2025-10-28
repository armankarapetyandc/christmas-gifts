using System;
using Cysharp.Threading.Tasks;
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

        public WeekSimulationV2 WeekSimulation { get; private set; }

        public WeekSimulationContext(WeekSimulationV2.Factory factory, PresenterService presenterService,AccountService accountService,
            NavigationPresenterService navigationPresenterService)
        {
            _factory = factory;
            _presenterService = presenterService;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
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
            WeekSimulation?.FinishWeek();
            _presenterService.HidePreviousAndShow<WeekReviewView>().Forget();
        }
    }
}