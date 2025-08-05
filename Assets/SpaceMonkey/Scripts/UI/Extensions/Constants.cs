using System.Collections.Generic;

namespace SpaceMonkey.Scripts.UI.Extensions
{
    public static class Constants
    {
        public static readonly float MaterialsCoefficientsMin = 0.35f;
        public static readonly float MaterialsCoefficientsMax = 5.5f;
        public static readonly float PackagingCoefficientsMin = 0.6f;
        public static readonly float PackagingCoefficientsMax = 2.75f;
        
        public static readonly Dictionary<string, float> TimeToProduct = new Dictionary<string, float>()
        {
            { "Diligent", 0.15f },
            { "Thoughtful", 0.08f },
            { "Average", 0.05f },
            { "Casual", 0.03f },
            { "Slipshod", 0.01f }
        };
    }
}