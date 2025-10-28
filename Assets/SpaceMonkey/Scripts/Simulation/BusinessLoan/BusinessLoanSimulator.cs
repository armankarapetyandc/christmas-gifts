using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using Zenject;
using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Simulation.BusinessLoan
{
    public class BusinessLoanSimulator
    {
        public const string LoanDescription = "Business Loan Payment";
        
        private GameConfig _config;
        private AccountService _accountService;
        public BusinessLoanDataGameData Data => _accountService.Model.Account.BusinessLoanData;
        private int Week => _accountService.Model.Account.Week;
        public bool HasActiveLoan => Data != null;
        public int PaymentIntervalWeeks => 4; // Payments every 4 weeks
        
        [Inject]
        private void Inject(GameConfig config, AccountService accountService)
        {
            _accountService = accountService;
            _config = config;
        }
        
        public async UniTask ApplyForLoan()
        {
            if (Data != null)
                return;

            // For demo, always approve with fixed terms
            float loanAmount = _config.BusinessLoanInfo.DefaultLoanAmount; // $7000
            float apr = _config.BusinessLoanInfo.DefaultAPR; // 12.5%
            
            _accountService.Model.Account.CreateBusinessLoanData(
                loanAmount, // Total loan amount
                loanAmount, // Initial balance is the full loan amount
                apr,
                _config.BusinessLoanInfo.DefaultTermMonths,
                Week // Track when the loan was taken
            );
            
            await _accountService.SaveAsync();
        }
        
        public float GetMonthlyPayment()
        {
            if (!HasActiveLoan)
                return 0;
                
            // For demo, use fixed principal and interest payments
            return _config.BusinessLoanInfo.FixedPrincipalPayment + _config.BusinessLoanInfo.FixedInterestPayment;
        }
        
        public int GetTotalPaymentsCount()
        {
            if (!HasActiveLoan)
                return 0;
                
            return _config.BusinessLoanInfo.DefaultTermMonths;
        }
        
        public float GetNextPaymentAmount()
        {
            if (!HasActiveLoan || Data.Balance <= 0)
                return 0;
                
            // For demo, use fixed principal and interest payments
            float principalPayment = Mathf.Min(_config.BusinessLoanInfo.FixedPrincipalPayment, Data.Balance);
            float interestPayment = _config.BusinessLoanInfo.FixedInterestPayment;
            
            return principalPayment + interestPayment;
        }
        
        public void NextWeek()
        {
            if (!HasActiveLoan || Data.Balance <= 0)
                return;
            
            // Process payment every 4 weeks (monthly payments)
            if ((Week - 1) % PaymentIntervalWeeks == 0)
            {
                ProcessMonthlyPayment();
                _accountService.SaveAsync().Forget();
            }
        }
        
        private void ProcessMonthlyPayment()
        {
            if (Data.Balance <= 0)
                return;
                
            // For demo, use fixed principal and interest payments
            float principalPayment = Mathf.Min(_config.BusinessLoanInfo.FixedPrincipalPayment, Data.Balance);
            float interestPayment = _config.BusinessLoanInfo.FixedInterestPayment;
            
            // Update the loan balance
            Data.Balance -= principalPayment;
            
            // Record the payment
            _accountService.Model.Account.AddLoanPaymentRecord(new PaymentRecord
            {
                Week = Week,
                Payment = principalPayment + interestPayment,
                Type = PaymentOption.Full, // Always full payment for loans
                Description = $"Loan Payment (Principal: ${principalPayment:F2}, Interest: ${interestPayment:F2})"
            });
            
            // If loan is paid off, clear the data
            if (Data.Balance <= 0.01f) // Small threshold to account for floating point errors
            {
                Data.Balance = 0;
                _accountService.Model.Account.BusinessLoanData = null;
            }
        }
        
        public async UniTask<bool> MakePayment(float amount, string description = "Loan Payment")
        {
            if (amount <= 0 || !HasActiveLoan)
                return false;
                
            // For loans, we typically don't allow partial payments, but we'll implement it anyway
            float payment = Mathf.Min(amount, Data.Balance);
            Data.Balance -= payment;
            
            _accountService.Model.Account.AddLoanPaymentRecord(new PaymentRecord
            {
                Week = Week,
                Payment = payment,
                Type = PaymentOption.Full,
                Description = description
            });
            
            await _accountService.SaveAsync();
            
            // If loan is paid off, clear the data
            if (Data.Balance <= 0.01f)
            {
                Data.Balance = 0;
                _accountService.Model.Account.BusinessLoanData = null;
                await _accountService.SaveAsync();
            }
            
            return true;
        }
        
        public float GetRemainingTermMonths()
        {
            if (!HasActiveLoan)
                return 0;
                
            // Calculate remaining term based on original term and payments made
            // This is a simplified calculation and may not be 100% accurate
            float monthlyPayment = GetMonthlyPayment();
            if (monthlyPayment <= 0)
                return 0;
                
            float remainingBalance = Data.Balance;
            float monthlyRate = Data.APR / 12f / 100f;
            
            // Calculate remaining months using the loan term formula
            // n = [ln(PMT) - ln(PMT - r * PV)] / ln(1 + r)
            if (monthlyPayment <= remainingBalance * monthlyRate)
                return float.PositiveInfinity; // Interest-only payments, will never pay off
                
            float numerator = Mathf.Log(monthlyPayment) - Mathf.Log(monthlyPayment - remainingBalance * monthlyRate);
            float denominator = Mathf.Log(1 + monthlyRate);
            
            return numerator / denominator;
        }
    }
}
