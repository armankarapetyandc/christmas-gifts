using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapPlaceHolderItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Sprite lockedIcon;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private Button button;
        private SpriteVisualAsset _visualAsset;
        public Observable<string> OnClickAsObservable() => button.OnClickAsObservable().Select(_ => nameText.text);
       
        public void SetPlaceName(string placeName)
        {
            nameText.SetText(placeName);
        }

        public void SetIcon(SpriteVisualAsset visualAsset)
        {
            _visualAsset = visualAsset;
            iconComponent.SetIcon(_visualAsset);
        }

        public void SetLocked(bool state)
        {
            // iconImage.sprite = state ? lockedIcon : _visualAsset?.Sprite;
        }
    }
}