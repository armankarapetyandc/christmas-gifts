using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using Zenject;

namespace SpaceMonkey.Scripts.Simulation.CreditCard
{
    using System;
    public class CreditSimulator
    {
        public const string ProdCapacityDescription = "Interest (APR 26%)";
        private static readonly Random Rng = new();

        private GameConfig _config;
        private AccountService _accountService;
        public CreditDataGameData Data => _accountService.Model.Account.CreditData;
        public bool HasActiveCard => Data != null;
        
        [Inject]
        private void Inject(GameConfig config, AccountService accountService)
        {
            _accountService = accountService;
            _config = config;
        }
        
        public void ApplyForCredit()
        {
            if (Data != null)
                return;

            _accountService.Model.Account.CreateCreditData(0, _config.CreditCardInfo.CreditLimit,
                _config.CreditCardInfo.Apr, 0);
            
            SelectPayment(PaymentOption.Skip);
            _accountService.SaveAsync().Forget();
        }
        
        public void SelectPayment(PaymentOption option)
        {
            Data.SelectedPayment = option;
        }

        public bool MakePurchase(float amount, string description)
        {
            if (amount <= 0 || Data.Balance + amount > Data.CreditLimit)
                return false;

            Data.Balance += amount;
            _accountService.Model.Account.AddCreditTransaction(new Transaction { Description = description, Amount = amount, Week = Data.Week });
            _accountService.SaveAsync().Forget();
            return true;
        }

        public bool AddCredit(float amount, string description)
        {
            if (amount <= 0)
                return false;

            Data.Balance -= amount;
            _accountService.Model.Account.AddCreditTransaction(new Transaction { Description = "Credit Added", Amount = -amount, Week = Data.Week });
            _accountService.SaveAsync().Forget();
            return true;
        }

        public float GetMinimumPayment()
        {
            if (Data == null || Data.Balance <= 0)
                return 0;

            return Math.Min(Data.Balance, Math.Max(_config.CreditCardInfo.MinimumPayment, Data.Balance * 
                _config.CreditCardInfo.MinimumPatmentCoff));
        }
        
        public float GetPLPaymentAmount()
        {
            if (HasActiveCard == false || Data.SelectedPayment == PaymentOption.None
                || Data.Balance <= 0)
            {
                return 0;
            }

            if ((Data.Week+1) % 4 != 0)
            {
                return 0;
            }
            return GetPaymentAmount(Data.SelectedPayment);
        }

        private float GetPaymentAmount(PaymentOption option)
        {
            return option switch
            {
                PaymentOption.Skip => 0,
                PaymentOption.Minimum => GetMinimumPayment(),
                PaymentOption.Full => Data.Balance,
                _ => 0
            };
        }
        
        public void NextWeek()
        {
            if(Data==null)
                return;
            
            if (Data.Balance > 0 && Data.SelectedPayment == PaymentOption.None)
                return;
                
            var weeklyRate = Data.Apr / 52f;
            var payment = 0f;

            if (Data.Balance > 0)
            {
                var interest = Data.Balance * weeklyRate;
                Data.Balance += interest;

                payment = GetPaymentAmount(Data.SelectedPayment);
                
                Data.Balance -= payment;
                _accountService.Model.Account.AddPaymentRecord(new PaymentRecord
                    { Week = Data.Week, Payment = payment, Type = Data.SelectedPayment });
                UpdateCreditScore(Data.SelectedPayment);
            }

            Data.Week++;
            _accountService.SaveAsync().Forget();
            
            if (Data.Balance <= 0)
            {
                ResetAsync();
            }
        }
        
        private void UpdateCreditScore(PaymentOption paymentType)
        {
            var scoreChange = 0f;

            // Payment history (35%)
            if (paymentType == PaymentOption.Skip)
                scoreChange -= RandomBetween(20, 50);
            else if (paymentType == PaymentOption.Minimum)
                scoreChange += RandomBetween(0, 5);
            else if (paymentType == PaymentOption.Full)
                scoreChange += RandomBetween(5, 15);

            // Credit utilization (30%)
            var utilization = Data.Balance / Data.CreditLimit;
            if (utilization > 0.9f) scoreChange -= RandomBetween(0, 15);
            else if (utilization > 0.7f) scoreChange -= RandomBetween(0, 10);
            else if (utilization > 0.3f) scoreChange -= RandomBetween(0, 5);
            else if (utilization < 0.1f) scoreChange += RandomBetween(0, 5);

            // Random variation
            scoreChange += RandomBetween(-5f, 5f);

            Data.CreditScore = Math.Max(300, Math.Min(850, (int)Math.Round(Data.CreditScore + scoreChange)));
            _accountService.SaveAsync().Forget();
        }

        private int RandomBetween(int min, int max)
        {
            return Rng.Next(min, max + 1);
        }

        private float RandomBetween(float min, float max)
        {
            return (float)(min + Rng.NextDouble() * (max - min));
        }

        private void ResetAsync()
        {
            _accountService.Model.Account.ResetCreditData();
            _accountService.SaveAsync().Forget();
        }
    }
}