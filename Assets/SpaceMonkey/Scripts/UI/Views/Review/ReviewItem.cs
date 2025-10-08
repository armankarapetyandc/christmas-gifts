using System;
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
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI commentText;
        [SerializeField] private RectTransform commentTextRect;
        [SerializeField] private Slider moodSlider;
        [SerializeField] private GameObject[] stars;

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
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(i + 1 <= value);
            }
        }

        private void Update()
        {
            layoutElement.preferredHeight =
                Mathf.Abs(commentTextRect.anchoredPosition.y) + commentTextRect.rect.height + 90f;
        }
    }
}