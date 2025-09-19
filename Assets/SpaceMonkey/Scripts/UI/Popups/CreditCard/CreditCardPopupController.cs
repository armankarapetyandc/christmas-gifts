using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCard
{
    public class CreditCardPopupController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        public CreditCardPopupController(PresenterService presenterService,NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
        }
        
        public void RedirectToCreditCardStatement()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<CreditCardView>().Forget();
        }
    }
}