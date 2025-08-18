using System;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class IconCollectionItemComponent : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image image;
        public SpriteVisualAsset VisualAsset { get; private set; }

        public Observable<SpriteVisualAsset> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b && VisualAsset != null).Select(_ => VisualAsset);

        private void Start()
        {
            if (transform.parent.TryGetComponent<ToggleGroup>(out var toggleGroup))
            {
                toggle.group = toggleGroup;
            }
        }

        internal void Setup(SpriteVisualAsset visualAsset)
        {
            VisualAsset = visualAsset;
            image.sprite = visualAsset.Sprite;
        }

        public void SetStateWithoutNotify(bool state)
        {
            toggle.SetIsOnWithoutNotify(state);
        }
        
        public void SetState(bool state)
        {
            toggle.isOn = state;
        }
    }
}