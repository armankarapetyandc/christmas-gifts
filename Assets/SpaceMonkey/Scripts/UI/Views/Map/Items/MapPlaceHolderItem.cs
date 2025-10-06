using R3;
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
        private SpriteVisualAsset _visualAsset;

        public virtual void Init()
        {
            
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