using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCard;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardDecline
{
    public class CreditCardDeclineViewController : BasePresenterController
    { 
        public CreditCardDeclineViewController(PresenterService presenterService) : base(presenterService)
        {
            
        }

        internal void OnBack()
        {
            PresenterService.Show<CreditCardView>().Forget();
        }
    }
}