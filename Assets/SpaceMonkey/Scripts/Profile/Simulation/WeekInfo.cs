using System;
using System.Collections.Generic;

namespace SpaceMonkey.Scripts.Profile.Simulation
{
    [Serializable]
    public struct CustomerReviewInfo
    {
        public string WeekId { get; set; }
        public string CharacterId { get; set; }
        public string ProductId { get; set; }
        public string Message { get; set; }
        public int StarRating { get; set; } // 1-5 stars based on mood
        public ReviewType Type { get; set; } // BigChange or ExtremeSetting
        public string TriggerReason { get; set; } // What caused the review (e.g., "Price -80%")
    }

    public sealed class CustomerReviewComparer : IEqualityComparer<CustomerReviewInfo>
    {
        public bool Equals(CustomerReviewInfo x, CustomerReviewInfo y)
        {
            return string.Equals(x.WeekId, y.WeekId, StringComparison.Ordinal) &&
                   string.Equals(x.CharacterId, y.CharacterId, StringComparison.Ordinal) &&
                   string.Equals(x.ProductId, y.ProductId, StringComparison.Ordinal);
        }

        public int GetHashCode(CustomerReviewInfo obj)
        {
            return HashCode.Combine(
                obj.WeekId,
                obj.CharacterId,
                obj.ProductId
            );
        }
    }
    
    public enum ReviewType
    {
        BigChange,
        ExtremeSettingHigh,
        ExtremeSettingLow
    }
}