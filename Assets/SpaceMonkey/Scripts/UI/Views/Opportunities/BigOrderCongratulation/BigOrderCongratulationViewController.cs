using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation
{
    public class BigOrderCongratulationViewController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;

        public BigOrderCongratulationViewController(PresenterService presenterService,NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }

        internal void OnNext(MainNavigationType Type)
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type =Type
            }).Forget();
            
            /*PresenterService.Show<BigOrderDetailsView>(new BigOrderDetailsView.Data()
            {
                Type = Type
            });*/
        }
    }
}