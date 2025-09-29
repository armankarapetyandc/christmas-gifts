using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BankAccounts;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using SpaceMonkey.Scripts.UI.Views.Opportunities.Insurance;
using SpaceMonkey.Scripts.UI.Views.Opportunities.Investing;
using SpaceMonkey.Scripts.UI.Views.Opportunities.MutualFunds;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Examples.Popups.TempWithDataAndController;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities
{
    public class OpportunitiesController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly CreditSimulator _creditSimulator;

        public OpportunitiesController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, CreditSimulator creditSimulator) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _creditSimulator = creditSimulator;
        }

        internal void OnMutualFundsButtonClicked()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<MutualFundsView>().Forget();
        }

        public void OnCreditCardButtonClicked()
        {
            _navigationPresenterService.HideAll();
            if (_creditSimulator.HasActiveCard)
            {
                PresenterService.HidePreviousAndShow<CreditCardStatementView>(new CreditCardStatementView.Data()).Forget();
                return;
            }
            PresenterService.HidePreviousAndShow<CreditCardView>().Forget();
        }
        
        public void OnInsuranceButtonClicked()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<InsuranceView>().Forget();
        }

        public void OnInvestmentButtonClicked()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<InvestingView>().Forget();
        }
        
        public void OnBankAccountButtonClicked()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<BankAccountsView>().Forget();
        }
    }
}