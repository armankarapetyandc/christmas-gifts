using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Review;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.WeekReview
{
    public class WeekReviewController : BasePresenterController
    {
        private PresenterService _presenterService;

        public WeekReviewController(PresenterService presenterService) :
            base(presenterService)
        {
            _presenterService = presenterService;
        }

        internal void OnNext()
        {
            _presenterService.HidePreviousAndShow<ReviewView>().Forget();
        }
    }
}