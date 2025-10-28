using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.DisasterInsurance;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BankAccounts;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsurancePolicy;
using SpaceMonkey.Scripts.UI.Views.Opportunities.Investing;
using SpaceMonkey.Scripts.UI.Views.Opportunities.MutualFunds;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Examples.Popups.TempWithAll;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities
{
    public class OpportunitiesController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly CreditSimulator _creditSimulator;
        private readonly BusinessLoanSimulator _businessLoanSimulator;
        private AccountService _accountService;
        private MapConfig _mapConfig;

        public OpportunitiesController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,
            CreditSimulator creditSimulator,
            BusinessLoanSimulator businessLoanSimulator,
            AccountService accountService, MapConfig mapConfig) : base(presenterService)
        {
            _mapConfig = mapConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
            _creditSimulator = creditSimulator;
            _businessLoanSimulator = businessLoanSimulator;
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
                PresenterService.HidePreviousAndShow<CreditCardStatementView>(new CreditCardStatementView.Data())
                    .Forget();
                return;
            }

            PresenterService.HidePreviousAndShow<CreditCardView>().Forget();
        }

        public void OnInsuranceButtonClicked()
        {
            _navigationPresenterService.HideAll();
            if (_accountService.Model.Account.InsuranceData != null)
            {
                PresenterService.HidePreviousAndShow<DisasterInsurancePolicyView>().Forget();
                return;
            }

            PresenterService.HidePreviousAndShow<DisasterInsuranceView>().Forget();
        }

        public void OnInvestmentButtonClicked()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<InvestingView>().Forget();
        }

        public void OnBankAccountButtonClicked()
        {
            _navigationPresenterService.HideAll();
            if (_businessLoanSimulator.HasActiveLoan)
            {
                PresenterService.HidePreviousAndShow<BusinessLoanStatementView>().Forget();
                return;
            }

            PresenterService.HidePreviousAndShow<BankAccountsView>().Forget();
        }

        public void OnBigOrderButtonClicked()
        {
            if (_mapConfig.Places.FirstOrDefault(place => place.Type == PlaceType.BigOrder)?.AppearLevel >
                _accountService.Model.Account.Level)
            {
                return;
            }

            _navigationPresenterService.HideAll();
            if (_accountService.Model.Account.BigOrderGameData != null)
            {
                PresenterService.HidePreviousAndShow<BigOrderDetailsView>(new BigOrderDetailsView.Data()
                {
                    Type = MainNavigationType.Opportunities
                }).Forget();
                return;
            }

            PresenterService.HidePreviousAndShow<BigOrderView>(new BigOrderView.Data()
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }
    }
}