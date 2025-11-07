using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    [System.Serializable]
    public class BusinessLoanInfo
    {
        [Header("Loan Terms")]
        [Tooltip("Default loan amount for the business loan")]
        public float DefaultLoanAmount = 7000f;
        
        [Tooltip("Default Annual Percentage Rate (APR) for the loan")]
        [Range(0.1f, 100f)]
        public float DefaultAPR = 12.5f; // 12.5% APR
        
        [Tooltip("Default loan term in months")]
        public int DefaultTermMonths = 60; // 5 years
        
        [Header("Payment Settings")]
        [Tooltip("Fixed principal payment amount per month")]
        public float FixedPrincipalPayment = 112f;
        
        [Tooltip("Fixed interest payment amount per month (for demo purposes)")]
        public float FixedInterestPayment = 87.50f;
        
        [Tooltip("Payment interval in weeks (e.g., 4 = monthly payments)")]
        public int PaymentIntervalWeeks = 4;
    }
}
