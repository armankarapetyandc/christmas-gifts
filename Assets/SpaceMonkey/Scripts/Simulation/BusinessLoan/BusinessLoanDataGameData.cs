using System.Collections.Generic;
using SpaceMonkey.Scripts.Profile;

namespace SpaceMonkey.Scripts.Simulation.BusinessLoan
{
    public class BusinessLoanDataGameData
    {
        public float OriginalAmount { get; set; }  // Original loan amount
        public float Balance { get; set; }         // Current remaining balance
        public float APR { get; set; }             // Annual Percentage Rate (e.g., 12.5 for 12.5%)
        public int TermMonths { get; set; }        // Loan term in months
        public int StartWeek { get; set; }         // Game week when the loan was taken
        public List<PaymentRecord> PaymentHistory { get; private set; } = new();

        public BusinessLoanDataGameData(float originalAmount, float balance, float apr, int termMonths, int startWeek)
        {
            OriginalAmount = originalAmount;
            Balance = balance;
            APR = apr;
            TermMonths = termMonths;
            StartWeek = startWeek;
        }
    }
}
