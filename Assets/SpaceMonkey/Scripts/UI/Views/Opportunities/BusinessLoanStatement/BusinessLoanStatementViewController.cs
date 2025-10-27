using System;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.BusinessLoan;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoan;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanSplash;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement
{
    public class BusinessLoanStatementViewController : BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly BusinessLoanSimulator _businessLoanSimulator;
        private readonly GameConfig _config;
        private readonly AccountService _accountService;

        public BusinessLoanStatementViewController(
            PresenterService presenterService, 
            PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService,
            BusinessLoanSimulator businessLoanSimulator,
            GameConfig config,
            AccountService accountService) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
            _businessLoanSimulator = businessLoanSimulator;
            _config = config;
            _accountService = accountService;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }

        internal async void OnNext()
        {
            // Check if already has an active loan
            if (_businessLoanSimulator.HasActiveLoan)
            {
                // Show loan details or payment screen
                PresenterService.Show<BusinessLoanStatementView>(new BusinessLoanStatementView.Data()).Forget();
                return;
            }
            
            // Show loan application splash screen
            PresenterService.Show<BusinessLoanSplashView>().Forget();
        }

        public void OnInfo()
        {
            // Show loan information popup
            _popupPresenterService.Show<BusinessLoanPopup>().Forget();
        }
        
        public (float amount, float apr, float monthlyPayment, float totalPayment) GetLoanDetails()
        {
            if (!_businessLoanSimulator.HasActiveLoan)
            {
                // Return default values for the loan offer
                float amount = _config.BusinessLoanInfo.DefaultLoanAmount;
                float apr = _config.BusinessLoanInfo.DefaultAPR;
                float monthlyPayment = _config.BusinessLoanInfo.FixedPrincipalPayment + _config.BusinessLoanInfo.FixedInterestPayment;
                float totalPayment = monthlyPayment * (_config.BusinessLoanInfo.DefaultTermMonths);
                
                return (amount, apr, monthlyPayment, totalPayment);
            }
            else
            {
                // Return current loan details
                return (
                    _businessLoanSimulator.Data.OriginalAmount,
                    _businessLoanSimulator.Data.APR,
                    _businessLoanSimulator.GetMonthlyPayment(),
                    _businessLoanSimulator.GetRemainingTermMonths()
                );
            }
        }
        public string GetCustomerName()
        {
            return _accountService?.Model?.Account?.Company?.CompanyName ?? "Customer";
        }
        
        public int GetNextPaymentDate()
        {
            if (!_businessLoanSimulator.HasActiveLoan)
                return 0;
                
            // Calculate weeks since loan start
            int currentWeek = _accountService.Model.Account.Week;
            int weeksSinceLoanStart = currentWeek - _businessLoanSimulator.Data.StartWeek;
            
            // Calculate weeks until next payment
            int weeksUntilNextPayment = _businessLoanSimulator.PaymentIntervalWeeks - (weeksSinceLoanStart % _businessLoanSimulator.PaymentIntervalWeeks);
            
            // Return the week number when the next payment is due
            return currentWeek + weeksUntilNextPayment;
        }
        
        public int GetMaturityDate()
        {
            if (!_businessLoanSimulator.HasActiveLoan)
                return 0; // Return 0 if no active loan
                
            // Calculate maturity week based on loan start week and term
            int totalWeeks = _businessLoanSimulator.Data.TermMonths * 4; // Approximate months to weeks
            return _businessLoanSimulator.Data.StartWeek + totalWeeks;
        }
        
        public int GetPaymentsMade()
        {
            if (!_businessLoanSimulator.HasActiveLoan)
                return 0;
                
            int weeksSinceLoanStart = _accountService.Model.Account.Week - _businessLoanSimulator.Data.StartWeek;
            return weeksSinceLoanStart / _businessLoanSimulator.PaymentIntervalWeeks;
        }
    }
}