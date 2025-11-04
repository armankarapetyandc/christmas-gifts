using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Views.Review;
using SpaceMonkey.Scripts.UI.Views.UpgradeCapacity;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.WeekReview
{
    public class WeekReviewController : BasePresenterController
    {
        private PresenterService _presenterService;
        private AccountService _accountService;
        private WeekSimulationContext _weekSimulationContext;

        public WeekReviewController(PresenterService presenterService, AccountService accountService,
            WeekSimulationContext weekSimulationContext) :
            base(presenterService)
        {
            _weekSimulationContext = weekSimulationContext;
            _accountService = accountService;
            _presenterService = presenterService;
        }

        internal void OnNext()
        {
            _weekSimulationContext.WeekSimulation?.FinishWeek();
        }
    }
}