using System.Net;
using System.Threading.Tasks;

namespace SpaceMonkey.Scripts.Profile
{
    public class Account
    {
        public CompanyInfo Company { get; set; }
        public uint Level { get; set; }
        public float Rating { get; set; }
    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; }
        public string Category { get; set; }
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