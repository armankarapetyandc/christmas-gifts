using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class ColorCollectionItemComponent : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image image;
        private ColorVisualAsset _visualAsset;

        public Observable<ColorVisualAsset> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b && _visualAsset != null).Select(_ => _visualAsset);

        private void Start()
        {
            if (transform.parent.TryGetComponent<ToggleGroup>(out var toggleGroup))
            {
                toggle.group = toggleGroup;
            }
        }

        internal void Setup(ColorVisualAsset visualAsset)
        {
            _visualAsset = visualAsset;
            image.color = visualAsset.Color;
        }
    }
}