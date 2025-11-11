using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Views.ProfitAndLoss;
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

        internal void OnNext(bool? levelIncreased)
        {
            if (levelIncreased != null && levelIncreased.Value)
            {
                _presenterService.HidePreviousAndShow<WeekEndRewardView>().Forget();
                return;
            }

            if (GetReviews()?.Count() > 0)
            {
                PresenterService.HidePreviousAndShow<ReviewView>(new ReviewView.Data()
                {
                    IsWeekEnd = true
                }).Forget();
                return;
            }

            PresenterService.Show<ProfitView>(new ProfitView.Data
            {
                UseSimulation = true,
                IsWeekEnd = true
            }).Forget();
        }

        private IEnumerable<CustomerReviewInfo> GetReviews()
        {
            return _accountService.Model.Account.Reviews?.Where(info =>
                info.WeekId.Equals(_weekSimulationContext.WeekSimulation.CurrentWeek.Value.Id));
        }
    }
}