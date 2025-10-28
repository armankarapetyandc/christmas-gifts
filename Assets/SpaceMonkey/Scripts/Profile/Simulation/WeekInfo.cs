using System;

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

    public enum ReviewType
    {
        BigChange,
        ExtremeSettingHigh,
        ExtremeSettingLow
    }
}