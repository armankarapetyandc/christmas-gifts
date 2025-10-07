using R3;
using SpaceMonkey.Scripts.Configs.Map;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Map.Items
{
    public class MapPlaceHolderItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject lockedState;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private Button button;
        [SerializeField] private ColorVisualAsset placeHolderFirstShowColor;
        [SerializeField] private ColorVisualAsset placeHolderDefaultColor;

        private SpriteVisualAsset _visualAsset;
        private PlaceType _placeType;

        public virtual void Init(PlaceType placeType)
        {
            _placeType = placeType;
            SetIconColor();
        }
        
        public void SetPlaceName(string placeName)
        {
            nameText.SetText(placeName);
        }

        public void SetIcon(SpriteVisualAsset visualAsset)
        {
            _visualAsset = visualAsset;
            iconComponent.SetIcon(_visualAsset);
        }

        private void SetIconColor()
        {
            var isFirstShow = PlayerPrefs.GetInt(_placeType.ToString(), 0) == 0;
            iconComponent.SetColor(isFirstShow ? placeHolderFirstShowColor : placeHolderDefaultColor);
            if (isFirstShow)
            {
                PlayerPrefs.SetInt(_placeType.ToString(), 1);
                PlayerPrefs.Save();
            }
        }

        protected void SetLocked(bool state)
        {
            iconComponent.gameObject.SetActive(!state);
            lockedState.gameObject.SetActive(state);
        }

        public Observable<Unit> GetClickHandler()
        {
            button.onClick.RemoveAllListeners();
            return button.OnClickAsObservable();
        }
    }
}