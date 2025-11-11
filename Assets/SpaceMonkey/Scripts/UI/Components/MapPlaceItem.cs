using System;
using R3;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.Core.SimpleSerializableDictionary;
using SpaceMonkey.Scripts.UI.Asset.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public enum MapPlaceState
    {
        Active,
        Selected,
        Locked,
        Open,
        Event,
        Inactive
    }
    public class MapPlaceItem : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button button;

        [SerializeField] private Image pinImage;
        [SerializeField] private Image iconRootImage;
        [SerializeField] private Image contentImage;
        [SerializeField] private SerializableDictionary<MapPlaceState, Sprite> statesMap;
        
        private SpriteVisualAsset _visualAsset;
        private IMapPlace _place;

        public Observable<IMapPlace> OnClickAsObservable() =>
            button.OnClickAsObservable().Where(_ =>!_place.DefaultLocked).Select(_ => _place);


        public void SetMapPlaceState(MapPlaceState placeState)
        {
            var map = statesMap.ToDictionary();
            if (!map.TryGetValue(placeState, out var state))
            {
                Debug.LogError($"Unable to find pin state image for state: {placeState}");
                return;
            }

            pinImage.sprite = state;
            iconRootImage.gameObject.SetActive(placeState != MapPlaceState.Locked);
        }


        public void SetPlaceContent(Sprite sprite)
        {
            contentImage.sprite = sprite;
        }
        public void SetBackgroundColor(Color color)
        {
            iconRootImage.color = color;
        }
        
        public void SetPlaceName(string placeName)
        {
            var formatted=System.Text.RegularExpressions.Regex.Replace(placeName, @"\s+", " ").Trim();
            nameText.SetText(formatted);
        }

        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }
        
        public void SetPlace(IMapPlace place)
        {
            _place = place;
        }
    }
}