using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation
{
    public class BigOrderCongratulationViewController : BasePresenterController
    {
        public BigOrderCongratulationViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        internal void OnNext(MainNavigationType Type)
        {
            PresenterService.Show<BigOrderDetailsView>(new BigOrderDetailsView.Data()
            {
                Type = Type
            });
        }
    }
}