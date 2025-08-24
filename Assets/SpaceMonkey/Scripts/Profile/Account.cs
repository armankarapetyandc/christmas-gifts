using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpaceMonkey.Scripts.Configs;

namespace SpaceMonkey.Scripts.Profile
{
    public class Account
    {
        public CompanyInfo Company { get; set; }
        public uint Level { get; set; }
        public uint Week { get; set; }
        public float Rating { get; set; }
        public float Money { get; set; }
        public float Score { get; set; }

        public List<Product> Products { get; set; }
        public List<LevelProdCap> LevelProdCaps { get; set; }

        public void SetCategory(string category)
        {
            Company.Category = category;
        }

        public void SetCompanyName(string companyName)
        {
            Company.CompanyName = companyName;
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
                Week = 1,
                Money = 30000,
                Rating = 0,
                Score = 0,
                Products = new List<Product>(),
                LevelProdCaps = new List<LevelProdCap>()
                {
                    new()
                    {
                        Id = initialProdCap.Id,
                        NeedRepair = false,
                        ProdCapCost = initialProdCap.ProdCapCost,
                        ProdCapAdd = initialProdCap.ProdCapAdd
                    }
                }
            };
            return account;
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

        public void Reset()
        {
            Company?.Reset();
            Level = 0;
            Money = 0;
            Score = 0;
            Products = new List<Product>();
            LevelProdCaps = new List<LevelProdCap>();
        }

        public void DeleteProduct(string productId)
        {
            Products.RemoveAll(p => p.Id.Equals(productId));
        }

        public int GetProductionCapacity()
        {
            return LevelProdCaps
                .Where(l => !l.NeedRepair)
                .Sum(l => l.ProdCapAdd);
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

    public struct Product
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
    }

    public struct LevelProdCap
    {
        public string Id { get; set; }
        public int ProdCapAdd {get; set;}
        public int ProdCapCost {get; set;}
        public bool NeedRepair { get; set; }
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
            return Equals((Hashtag)obj);
        }

        public override int GetHashCode()
        {
            return Tag != null ? Tag.GetHashCode() : 0;
        }
    }
}