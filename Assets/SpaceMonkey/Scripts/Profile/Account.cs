namespace SpaceMonkey.Scripts.Profile
{
    public class Account
    {
        public CompanyInfo Company { get; set; }
        public uint Level { get; set; }
        public float Rating { get; set; }

        public void SetCategory(string category, string categoryId)
        {
            Company.Category = category;
            Company.CategoryId = categoryId;
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
                }
            };
            return account;
        }

        public void SetCompanyLogo(string shapeSpriteName, string iconSpriteName, string backgroundColorHex)
        {
            Company.Logo.Shape = shapeSpriteName;
            Company.Logo.Icon = iconSpriteName;
            Company.Logo.Background = backgroundColorHex;
        }

        public void SetTags(string[] tags)
        {
            Company.HashTags = tags;
        }
    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; }
        public string Category { get; set; }

        public string CategoryId { get; set; }
        public CompanyLogo Logo { get; set; }
        public string[] HashTags { get; set; }
    }

    public class CompanyLogo
    {
        public string Shape { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
    }
}