using System;
using SpaceMonkey.Scripts.Configs;

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
                    Logo = new CompanyLogo()
                },
                Level = 1,
                Money = 300,
                ProductionCapacity = 5,
                Rating = 0,
                Score = 0
            };
            return account;
        }

        public void SetCompanyLogo(string shapeSpriteName, string iconSpriteName, string backgroundColorHex)
        {
            Company.Logo.Shape = shapeSpriteName;
            Company.Logo.Icon = iconSpriteName;
            Company.Logo.Background = backgroundColorHex;
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
        public string Shape { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
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