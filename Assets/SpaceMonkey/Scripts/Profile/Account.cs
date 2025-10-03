using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Views.Staff;
using SpaceMonkey.Scripts.Utilities;

namespace SpaceMonkey.Scripts.Profile
{
    public class Account
    {
        private float _score;
        public CompanyInfo Company { get; set; }
        public int Level { get; set; }
        public int Week => Weeks.Count + 1;
        public float Money { get; set; }

        public float Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnScoreChanged.Execute(_score);
            }
        }
        
        public readonly ReactiveCommand<float> OnScoreChanged = new ReactiveCommand<float>();


        public List<Product> Products { get; set; }
        public List<LevelProdCap> LevelProdCaps { get; set; }
        public List<WeekInfo> Weeks { get; set; }
        public List<CustomerReviewInfo> Reviews { get; set; }
        public List<MarketingFeature> MarketingFeatures { get; set; }

        public List<Employee> Employees { get; set; }
        
        public CreditDataGameData CreditData { get; set; }

        public void SetCategory(string category)
        {
            Company.Category = category;
        }

        public void SetCompanyName(string companyName)
        {
            Company.CompanyName = companyName;
        }

        public float CalculateCompanyRating(RangeValue[] values)
        {
            return Weeks
                .SelectMany(w => w.Orders)
                .Select(o =>
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (o.Mood >= values[i].Min && o.Mood <= values[i].Max)
                        {
                            return i + 1f;
                        }
                    }

                    throw new ArgumentOutOfRangeException(nameof(o.Mood), "Mood value must be between 1 and 100.");
                }).Average();
        }

        public static Account CreateEmpty(ProductionLevelInfo initialProdCap)
        {
            var account = new Account
            {
                Company = new CompanyInfo
                {
                    Logo = new CompanyLogo(),
                    Tags = Array.Empty<Hashtag>()
                },
                Level = 1,
                Money = 30000,
                Score = 0,
                Products = new List<Product>(),
                Weeks = new List<WeekInfo>(),
                Reviews = new List<CustomerReviewInfo>(),
                LevelProdCaps = new List<LevelProdCap>()
                {
                    new()
                    {
                        Id = initialProdCap.Id,
                        NeedRepair = false,
                        ProdCapCost = initialProdCap.ProdCapCost,
                        ProdCapAdd = initialProdCap.ProdCapAdd
                    }
                },
                MarketingFeatures = new List<MarketingFeature>(),
                Employees = new List<Employee>(),
                CreditData = null
            };
            return account;
        }

        public void PushFinishedWeek(WeekInfo info)
        {
            Weeks.Add(info);
        }

        public bool CanAfford(float cost)
        {
            return Money >= cost;
        }

        public void Buy(float cost)
        {
            if (!CanAfford(cost))
            {
                throw new Exception("Not enough money");
            }

            Money -= cost;
        }

        public void Earn(float amount)
        {
            Money += amount;
        }

        public void SetCompanyLogo(CompanyLogo logo)
        {
            Company.Logo.BackgroundColorVisualAssetId = logo.BackgroundColorVisualAssetId;
            Company.Logo.IconVisualAssetId = logo.IconVisualAssetId;
            Company.Logo.ShapeVisualAssetId = logo.ShapeVisualAssetId;
        }

        public void SetTags(Hashtag[] tags)
        {
            Company.Tags = tags;
        }

        public void SetProduct(Product product)
        {
            int index = Products.FindIndex(p => p.Id.Equals(product.Id));
            if (index < 0)
            {
                Products.Add(product);
            }
            else
            {
                Products[index] = product;
                // Cascade update into Weeks -> Orders -> Products
                for (int w = 0; w < Weeks.Count; w++)
                {
                    var week = Weeks[w]; // struct copy
                    for (int o = 0; o < week.Orders.Length; o++)
                    {
                        var order = week.Orders[o]; // struct copy
                        for (int p = 0; p < order.Products.Length; p++)
                        {
                            var poi = order.Products[p]; // struct copy
                            if (poi.Product.Id == product.Id)
                            {
                                poi.Product = product; // update
                                order.Products[p] = poi; // put back
                            }
                        }

                        week.Orders[o] = order; // put back
                    }

                    Weeks[w] = week; // put back
                }
            }
        }

        public void SetLevel(LevelProdCap level)
        {
            int index = LevelProdCaps.FindIndex(p => p.Id.Equals(level.Id));
            if (index < 0)
            {
                LevelProdCaps.Add(level);
            }
            else
            {
                LevelProdCaps[index] = level;
            }
        }

        public void SetEmployee(Employee employee)
        {
            int index = Employees.FindIndex(p => p.Id.Equals(employee.Id));
            if (index < 0)
            {
                Employees.Add(employee);
            }
            else
            {
                Employees[index] = employee;
            }
        }

        public void FireEmployee(string firedEmployeeId)
        {
            Employees.RemoveAll(e => e.Id == firedEmployeeId);
        }

        public void Reset()
        {
            Company?.Reset();
            Level = 0;
            Money = 0;
            Score = 0;
            Products = new List<Product>();
            LevelProdCaps = new List<LevelProdCap>();
            MarketingFeatures = new List<MarketingFeature>();
            Employees = new List<Employee>();
            CreditData = null;
        }

        public void DeleteProduct(string productId)
        {
            Products.RemoveAll(p => p.Id.Equals(productId));
        }
        
        public void ResetCreditData()
        {
            CreditData = null;
        }
        
        public void CreateCreditData(float balance, float creditLimit, float apr, int creditScore)
        {
            CreditData = new CreditDataGameData(balance, creditLimit, apr, creditScore);
        }
        
        public void AddCreditTransaction(Transaction transaction)
        {
            CreditData?.Transactions.Add(transaction);
        }
        
        public void AddPaymentRecord(PaymentRecord record)
        {
            CreditData?.PaymentHistory.Add(record);
        }

        public int GetProductionCapacity()
        {
            var employeeCapacity = Employees.Sum(e => e.Capacity);
            var prodCap = LevelProdCaps
                .Where(l => !l.NeedRepair)
                .Sum(l => l.ProdCapAdd);
            return employeeCapacity + prodCap;
        }

        public float GetMarketingCustAdd()
        {
            return MarketingFeatures
                .Sum(feature => feature.GetCustAdd());
        }

        public Product GetProduct(string id)
        {
            return Products.Find(p => p.Id.Equals(id));
        }

        public void UpdateMarketingFeatures(List<MarketingFeature> marketingFeatures)
        {
            if (marketingFeatures == null) return;

            MarketingFeatures.RemoveAll(f => marketingFeatures.All(nf => nf.Id != f.Id));


            foreach (var feature in marketingFeatures)
            {
                var index = MarketingFeatures.FindIndex(f => f.Id == feature.Id);

                if (index == -1)
                {
                    MarketingFeatures.Add(feature);
                }
                else
                {
                    if (!MarketingFeatures[index].Equals(feature))
                    {
                        MarketingFeatures[index] = feature;
                    }
                }
            }
        }
    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; }
        public string Category { get; set; }
        public CompanyLogo Logo { get; set; }
        public Hashtag[] Tags { get; set; }

        public void Reset()
        {
            CompanyName = Category = null;
            Logo?.Reset();
            Tags = Array.Empty<Hashtag>();
        }
    }

    public class CompanyLogo
    {
        public string ShapeVisualAssetId { get; set; }
        public string IconVisualAssetId { get; set; }
        public string BackgroundColorVisualAssetId { get; set; }

        public void CopyFrom(CompanyLogo logo)
        {
            ShapeVisualAssetId = logo.ShapeVisualAssetId;
            IconVisualAssetId = logo.IconVisualAssetId;
            BackgroundColorVisualAssetId = logo.BackgroundColorVisualAssetId;
        }

        public void Reset()
        {
            ShapeVisualAssetId = IconVisualAssetId = BackgroundColorVisualAssetId = null;
        }
    }

    public struct Product : IEquatable<Product>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IconVisualAssetId { get; set; }
        public string BackgroundColorVisualAssetId { get; set; }

        public int TimeToProduceIndex { get; set; }
        public float? MaterialPrice { get; set; }
        public float? MaterialPackagingPrice { get; set; }
        public float? MinProductPrice { get; set; }
        public float? MaxProductPrice { get; set; }
        public float? ProductPrice { get; set; }

        public float? ShippingCost { get; set; }
        public float? Profit { get; set; }
        public float? ProdCapCost { get; set; }

        [JsonIgnore] public bool IsValid => !string.IsNullOrWhiteSpace(Id);

        public static Product CreateEmpty()
        {
            return new Product
            {
                TimeToProduceIndex = 1,
            };
        }

        public void AssignId()
        {
            Id = Guid.NewGuid().ToString();
        }

        // ✅ Equality based only on Id
        public bool Equals(Product other) => string.Equals(Id, other.Id, StringComparison.Ordinal);

        public override bool Equals(object obj) => obj is Product other && Equals(other);

        public override int GetHashCode() => (Id != null ? Id.GetHashCode() : 0);

        public static bool operator ==(Product left, Product right) => left.Equals(right);

        public static bool operator !=(Product left, Product right) => !left.Equals(right);
    }

    public struct LevelProdCap
    {
        public string Id { get; set; }
        public int ProdCapAdd { get; set; }
        public int ProdCapCost { get; set; }
        public bool NeedRepair { get; set; }
        public int LevelNumber { get; set; }
    }

    public struct MarketingFeature : IEquatable<MarketingFeature>
    {
        public string Id { get; set; }
        public float MinMult { get; set; }
        public float MaxMult { get; set; }
        public float CurrentPrice { get; set; }
        public int Division { get; set; }
        public int Unlock { get; set; }

        public bool Equals(MarketingFeature other)
        {
            return Id == other.Id && MinMult.Equals(other.MinMult) && MaxMult.Equals(other.MaxMult) &&
                   CurrentPrice.Equals(other.CurrentPrice) && Division == other.Division;
        }

        public override bool Equals(object obj)
        {
            return obj is MarketingFeature other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, MinMult, MaxMult, CurrentPrice, Division);
        }

        public float GetCustAdd()
        {
            return CurrentPrice / Division;
        }
    }

    public class Employee
    {
        public string Id { get; set; }
        public EmployeeProfession Profession { get; set; }
        public int Payroll { get; set; }
        public int Capacity { get; set; }
        public int Speed { get; set; }
        public int Experience { get; set; }
        public string CharacterId { get; set; }
    }

    public class Hashtag : IEquatable<Hashtag>
    {
        public string Tag { get; set; }
        public float MaterialAdd { get; set; }
        public float PackagingAdd { get; set; }

        public static Hashtag FromHashtagInfo(HashtagInfo info)
        {
            return new Hashtag
            {
                Tag = info.Tag,
                MaterialAdd = info.MaterialAdd,
                PackagingAdd = info.PackagingAdd
            };
        }

        public bool Equals(Hashtag other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Tag == null && other.Tag == null) return true;
            if (Tag == null || other.Tag == null) return false;
            return Tag == other.Tag;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Hashtag) obj);
        }

        public override int GetHashCode()
        {
            return Tag != null ? Tag.GetHashCode() : 0;
        }
    }
    
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

    public class CreditDataGameData
    {
        public float Balance { get; set; }
        public float CreditLimit { get; set; }
        public float Apr { get; set; }
        public int CreditScore { get; set; }
        public PaymentOption SelectedPayment { get; set; }
        
        public List<Transaction> Transactions { get; private set; } = new();
        public List<PaymentRecord> PaymentHistory { get; private set; } = new();

        public CreditDataGameData(float balance, float creditLimit, float apr, int creditScore)
        {
            Balance = balance;
            CreditLimit = creditLimit;
            Apr = apr;
            CreditScore = creditScore;
            SelectedPayment = PaymentOption.None;
        }
    }
}