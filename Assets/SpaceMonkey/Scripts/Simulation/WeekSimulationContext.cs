using System;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Orders;
using SpaceMonkey.Scripts.UI.Views.WeekReview;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;

namespace SpaceMonkey.Scripts.Simulation
{
    public class WeekSimulationContext : IDisposable
    {
        private readonly WeekSimulation.Factory _factory;
        private readonly PresenterService _presenterService;
        private readonly NavigationPresenterService _navigationPresenterService;

        public WeekSimulation WeekSimulation { get; private set; }

        public WeekSimulationContext(WeekSimulation.Factory factory, PresenterService presenterService,
            NavigationPresenterService navigationPresenterService)
        {
            _factory = factory;
            _presenterService = presenterService;
            _navigationPresenterService = navigationPresenterService;
        }

        public void Run()
        {
            WeekSimulation?.Dispose();
            WeekSimulation = _factory.Create();

            WeekSimulation.Prepare();
            WeekSimulation.Run();

            _navigationPresenterService.HideAll();
            _presenterService.HidePreviousAndShow<OrdersView>().Forget();
        }

        public bool TryShipOrder(Customer customer)
        {
            return WeekSimulation.TryShipOrder(customer);
        }
        public void Dispose()
        {
            WeekSimulation?.Dispose();
        }

        public void Finish()
        {
            WeekSimulation?.Finish();
            _presenterService.HidePreviousAndShow<WeekReviewView>().Forget();
        }
    }
}