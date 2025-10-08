using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Review
{
    public class ReviewWeekInfoItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ratingText;
        [SerializeField] private TextMeshProUGUI weekText;

        public void Initialize(int week, float rating)
        {
            weekText.text = $"Week {week}";
            ratingText.text = $"{rating} Rating";
        }
    }
}