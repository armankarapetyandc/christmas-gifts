using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    public enum ReviewType
    {
        ExtremeLowMaterial,
        ExtremeHighMaterial,
        ExtremeLowPackaging,
        ExtremeHighPackaging,
        ExtremeHighPrice,
        ExtremeLowPrice,
        ExtremeSlipshodTTP,
        ExtremeDiligentTTP,
        BigChangeMaterial,
        BigChangePackaging,
        BigChangeTTP
    }

    [CreateAssetMenu(fileName = "CustomerReviewConfig", menuName = "Space Monkey/Configs/Customer Review Config",
        order = 1)]
    public class CustomerReviewConfig : ScriptableObject
    {
        [field: SerializeField] public ReviewInfo[] Reviews { get; private set; }

        public void Set(ReviewInfo[] i)
        {
            Reviews = i;
        }
    }

    [Serializable]
    public class ReviewInfo
    {
        [field: SerializeField] public ReviewType Type { get; private set; }
        [field: SerializeField] public string Message { get; private set; }
        [field: SerializeField] public bool NeedFormating { get; private set; }

        public ReviewInfo()
        {
            
        }

        public ReviewInfo(ReviewType type, string message, bool needFormating)
        {
            Type = type;
            Message = message;
            NeedFormating = needFormating;
        }
        
        public string GetMessage(params string[] args)
        {
            if (NeedFormating && args == null)
            {
                throw new ArgumentException("Args must not be null when NeedFormating is true");
            }

            return NeedFormating ? string.Format(Message, args) : Message;
        }
    }
}