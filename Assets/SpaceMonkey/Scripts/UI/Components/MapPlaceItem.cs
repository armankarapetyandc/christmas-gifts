using SpaceMonkey.Scripts.UI.Asset.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class MapPlaceItem : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Sprite lockedIcon;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        private SpriteVisualAsset _visualAsset;

        public void SetPlaceName(string placeName)
        {
            nameText.SetText(placeName);
        }

        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }

        public void SetIcon(SpriteVisualAsset visualAsset)
        {
            _visualAsset = visualAsset;
            iconImage.sprite = visualAsset?.Sprite;
        }

        public void SetLocked(bool state)
        {
            iconImage.sprite = state ? lockedIcon : _visualAsset?.Sprite;
        }
    }
}