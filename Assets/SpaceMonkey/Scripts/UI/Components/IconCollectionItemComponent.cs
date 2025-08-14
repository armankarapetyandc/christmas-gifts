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
        private SpriteVisualAsset _visualAsset;

        public Observable<SpriteVisualAsset> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b && _visualAsset != null).Select(_ => _visualAsset);

        private void Start()
        {
            if (transform.parent.TryGetComponent<ToggleGroup>(out var toggleGroup))
            {
                toggle.group = toggleGroup;
            }
        }

        internal void Setup(SpriteVisualAsset visualAsset)
        {
            _visualAsset = visualAsset;
            image.sprite = visualAsset.Sprite;
        }
    }
}