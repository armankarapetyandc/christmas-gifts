using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Simulation.CreditCard
{
    using System;
    using System.Collections.Generic;

    public enum PaymentOption
    {
        None,
        Skip,
        Minimum,
        Full
    }

    public class Transaction
    {
        public string Description { get; set; }
        public float Amount { get; set; }
        public int Week { get; set; }
    }

    public class PaymentRecord
    {
        public int Week { get; set; }
        public float Payment { get; set; }
        public PaymentOption Type { get; set; }
    }

    public class GameData
    {
        public float Balance { get; set; }
        public float CreditLimit { get; set; }
        public float Apr { get; set; }
        public int Week { get; set; }
        public int CreditScore { get; set; }
        public PaymentOption SelectedPayment { get; set; }

        public GameData(float balance, float creditLimit, float apr, int creditScore)
        {
            Balance = balance;
            CreditLimit = creditLimit;
            Apr = apr;
            CreditScore = creditScore;
            Week = 1;
            SelectedPayment = PaymentOption.None;
        }
    }

    public class CreditSimulator : IInitializable
    {
        private const string Filename = "CreditSimulator.spacemonkey";
        private static readonly string Path = System.IO.Path.Combine(Application.persistentDataPath, Filename);
        private static readonly Random Rng = new();

        public GameData Data { get; private set; }
        public List<Transaction> Transactions { get; private set; } = new();
        public List<PaymentRecord> PaymentHistory { get; private set; } = new();
        
        public bool HasActiveCard => Data != null;

        public void Initialize()
        {
            LoadAsync().Forget();
        }
        
        public void ApplyForCredit()
        {
            if (Data != null)
                return;
            
            Data = new GameData(0, 1000, 0.26f, 0);
            SelectPayment(PaymentOption.Skip);
            
            Transactions.Clear();
            PaymentHistory.Clear();
            SaveAsync().Forget();
        }
        
        public void SelectPayment(PaymentOption option)
        {
            Data.SelectedPayment = option;
        }

        public bool MakePurchase(float amount)
        {
            if (amount <= 0 || Data.Balance + amount > Data.CreditLimit)
                return false;

            Data.Balance += amount;
            Transactions.Add(new Transaction { Description = "Purchase", Amount = amount, Week = Data.Week });
            SaveAsync().Forget();
            return true;
        }

        public bool AddCredit(float amount)
        {
            if (amount <= 0)
                return false;

            Data.Balance -= amount;
            Transactions.Add(new Transaction { Description = "Credit Added", Amount = -amount, Week = Data.Week });
            SaveAsync().Forget();
            return true;
        }

        public double GetMinimumPayment()
        {
            if (Data == null || Data.Balance <= 0)
                return 0;

            return Math.Min(Data.Balance, Math.Max(35, Data.Balance * 0.03f));
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

                var minPayment = Math.Min(Data.Balance, Math.Max(10, Data.Balance * 0.02f));

                switch (Data.SelectedPayment)
                {
                    case PaymentOption.Skip:
                        payment = 0;
                        break;
                    case PaymentOption.Minimum:
                        payment = minPayment;
                        break;
                    case PaymentOption.Full:
                        payment = Data.Balance;
                        break;
                }

                Data.Balance -= payment;
                PaymentHistory.Add(new PaymentRecord
                    { Week = Data.Week, Payment = payment, Type = Data.SelectedPayment });
                UpdateCreditScore(Data.SelectedPayment, payment, minPayment);
            }

            Data.Week++;
            SaveAsync().Forget();

            if (Data.Balance <= 0)
            {
                ResetAsync();
            }
        }

        private void UpdateCreditScore(PaymentOption paymentType, float payment, float minPayment)
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
            SaveAsync().Forget();
        }

        private int RandomBetween(int min, int max)
        {
            return Rng.Next(min, max + 1);
        }

        private float RandomBetween(float min, float max)
        {
            return (float)(min + Rng.NextDouble() * (max - min));
        }

        private async UniTask SaveAsync()
        {
            if (Data == null)
            {
                throw new Exception("Credit data is null");
            }

            var content = JsonConvert.SerializeObject(Data);
            await File.WriteAllTextAsync(Path, content);
        }

        private void ResetAsync()
        {
            Data = null;
            Transactions.Clear();
            PaymentHistory.Clear();
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        private async UniTask LoadAsync()
        {
            if (!File.Exists(Path))
            {
                return;
            }

            var content = await File.ReadAllTextAsync(Path);
            Data = JsonConvert.DeserializeObject<GameData>(content);
        }
    }
}