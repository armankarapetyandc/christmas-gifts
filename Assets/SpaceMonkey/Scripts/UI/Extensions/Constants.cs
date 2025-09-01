using System.Collections.Generic;

namespace SpaceMonkey.Scripts.UI.Extensions
{
    public static class Constants
    {
        public static readonly Dictionary<string, float> TimeToProduct = new Dictionary<string, float>()
        {
            { "Diligent", 0.15f },
            { "Thoughtful", 0.08f },
            { "Average", 0.05f },
            { "Casual", 0.03f },
            { "Slipshod", 0.01f }
        };

        public static readonly List<string> PriorityCategories = new List<string>()
        {
            "Cooking"
        };

        public static readonly Dictionary<int, (float, float)> MarketingSliderInit =
            new Dictionary<int, (float, float)>()
            {
                { 0, (2.00f, 25.00f) },
                { 1, (10.00f, 200.00f) },
                { 2, (8.00f, 75.00f) }
            };
    }
}