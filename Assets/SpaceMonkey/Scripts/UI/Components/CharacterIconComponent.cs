using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class CharacterIconComponent : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image moodImage;
        public SpriteVisualAsset MoodSpriteVisualAsset { get; private set; }
        public SpriteVisualAsset SpriteVisualAsset { get; private set; }
        public ColorVisualAsset ColorVisualAsset { get; private set; }

        public void SetColor(ColorVisualAsset asset)
        {
            if (asset != null)
            {
                ColorVisualAsset = asset;
                backgroundImage.color = asset.Color;
            }

            backgroundImage.gameObject.SetActive(asset != null);
        }

        public void SetIcon(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                SpriteVisualAsset = asset;
                iconImage.sprite = asset.Sprite;
            }

            iconImage.gameObject.SetActive(asset != null);
        }
        
        public void SetMoodIcon(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                MoodSpriteVisualAsset = asset;
                moodImage.sprite = asset.Sprite;
            }

            moodImage.gameObject.SetActive(asset != null);
        }
    }
}