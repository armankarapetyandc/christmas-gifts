using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoan;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLoan
{
    public class BusinessLoanPopupController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly BusinessLoanSimulator _businessLoanSimulator;
        private readonly NavigationPresenterService _navigationPresenterService;
        public BusinessLoanPopupController(PresenterService presenterService,
            PopupPresenterService popupPresenterService,
            BusinessLoanSimulator businessLoanSimulator,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
            _businessLoanSimulator = businessLoanSimulator;
            _navigationPresenterService = navigationPresenterService;
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
        
        public void RedirectToBusinessLoanStatement()
        {
            _navigationPresenterService.HideAll();

            if (_businessLoanSimulator.HasActiveLoan)
            {
                PresenterService.HidePreviousAndShow<BusinessLoanStatementView>().Forget();
                return;
            }
            PresenterService.HidePreviousAndShow<BusinessLoanView>().Forget();
        }
    }
}