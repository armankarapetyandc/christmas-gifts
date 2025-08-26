using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Review
{
    public class ReviewItem : MonoBehaviour
    {
        [SerializeField] private CharacterIconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI commentText;
        [SerializeField] private Slider moodSlider;
        [SerializeField] private Slider ratingSlider;
        
        public void SetCharacterVisual(SpriteVisualAsset characterVisual = null,
            ColorVisualAsset baseColorVisual = null)
        {
            iconComponent.SetIcon(characterVisual);
            iconComponent.SetColor(baseColorVisual);
        }

        public void SetTextData(string itemName, string comment)
        {
            nameText.text = itemName;
            commentText.text = comment;
        }

        public void SetMood(float value)
        {
            moodSlider.value = value;
        }

        public void SetRating(float value)
        {
            ratingSlider.value = value;
        }
    }
}