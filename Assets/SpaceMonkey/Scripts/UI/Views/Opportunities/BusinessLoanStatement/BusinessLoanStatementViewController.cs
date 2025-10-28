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
        
        public void OnInfo()
        {
            // Show loan information popup
            _popupPresenterService.Show<BusinessLoanPopup>().Forget();
        }
        
        public int GetTotalPaymentsCount()
        {
            return _businessLoanSimulator.GetTotalPaymentsCount();
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
            return _businessLoanSimulator.GetNextPaymentDate();
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
        
        public (float principal, float interest, float fee, float total) GetLastMonthPayment()
        {
            if (!_businessLoanSimulator.HasActiveLoan)
                return (0, 0, 0, 0);
                
            // Check if at least one payment has been made
            int paymentsMade = GetPaymentsMade();
            if (paymentsMade == 0)
                return (0, 0, 0, 0);
                
            // Get the fixed payment amounts from config
            float principal = _config.BusinessLoanInfo.FixedPrincipalPayment;
            float interest = _config.BusinessLoanInfo.FixedInterestPayment;
            float fee = 0; // No fees in the current implementation
            
            // If the loan balance is less than the principal payment, adjust it
            if (_businessLoanSimulator.Data.Balance + principal < principal)
            {
                principal = _businessLoanSimulator.Data.Balance + principal;
            }
            
            float total = principal + interest + fee;
            
            return (principal, interest, fee, total);
        }
    }
}