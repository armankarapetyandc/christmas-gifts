using System;
using System.Collections.Generic;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Views.Product;
using UnityEngine;

namespace SpaceMonkey.Scripts.Profile
{
    public class Account
    {
        public CompanyInfo Company { get; set; }
        public uint Level { get; set; }
        public float Rating { get; set; }
        public float Money { get; set; }
        public float ProductionCapacity { get; set; }
        public float Score { get; set; }

        public List<Product> Products { get; set; }

        public void SetCategory(string category)
        {
            Company.Category = category;
        }

        public void SetCompanyName(string companyName)
        {
            Company.CompanyName = companyName;
        }

        public static Account CreateEmpty()
        {
            var account = new Account
            {
                Company = new CompanyInfo
                {
                    Logo = new CompanyLogo(),
                    Tags = Array.Empty<Hashtag>()
                },
                Level = 1,
                Money = 300,
                ProductionCapacity = 5,
                Rating = 0,
                Score = 0,
                Products = new List<Product>()
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
    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; }
        public string Category { get; set; }
        public CompanyLogo Logo { get; set; }
        public Hashtag[] Tags { get; set; }
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
    }

    public struct Product
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public string IconVisualAssetId { get; set; }
        public string BackgroundColorVisualAssetId { get; set; }
        public float PackagingCost { get; set; }
        public float MaterialCost { get; set; }
        public float TotalCost { get; set; }
        public float ShippingCost { get; set; }
        public float Price { get; set; }
        public float TtpCost { get; set; }
        public float Profit { get; set; }
        public float TimeToProduceIndex { get; set; }

        public static Product CreateEmpty()
        {
            return new Product
            {
                Id = Guid.NewGuid().ToString()
            };
        }
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