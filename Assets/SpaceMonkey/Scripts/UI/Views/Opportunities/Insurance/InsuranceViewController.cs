using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.Insurance
{
    public class InsuranceViewController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public InsuranceViewController(PresenterService presenterService,NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }
        
    }
}