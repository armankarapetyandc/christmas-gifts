using System;
using R3;
using SpaceMonkey.Scripts.Configs.Map;
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
        [SerializeField] private Image background;
        [SerializeField] private Button button;
        private SpriteVisualAsset _visualAsset;
        private IMapPlace _place;
        public Observable<IMapPlace> OnClickAsObservable() => button.OnClickAsObservable().Select(_ => _place);
        public void SetPlaceName(string placeName)
        {
            var a=System.Text.RegularExpressions.Regex.Replace(placeName, @"\s+", " ").Trim();
            nameText.SetText(a);
        }

        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }

        public void SetBackgroundColor(Color color)
        {
            background.color = color;
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

        public void SetPlace(IMapPlace place)
        {
            _place = place;
        }
    }
}