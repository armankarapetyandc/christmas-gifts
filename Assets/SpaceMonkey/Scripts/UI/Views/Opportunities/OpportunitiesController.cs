using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.MutualFunds;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities
{
    public class OpportunitiesController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public OpportunitiesController(PresenterService presenterService,NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        internal void OnMutualFundsButtonClicked()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<MutualFundsView>().Forget();
        }
    }
}