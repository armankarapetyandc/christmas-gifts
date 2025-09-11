using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash
{
    public class CreditCardSplashViewController : BasePresenterController
    {
        public CreditCardSplashViewController(PresenterService presenterService) : base(presenterService)
        {
        }

        internal void OnNext()
        {
            PresenterService.Show<CreditCardView>().Forget();
        }
    }
}