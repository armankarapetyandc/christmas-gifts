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
        public ColorVisualAsset VisualAsset { get; private set; }

        public Observable<ColorVisualAsset> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b && VisualAsset != null).Select(_ => VisualAsset);

        private void Start()
        {
            if (transform.parent.TryGetComponent<ToggleGroup>(out var toggleGroup))
            {
                toggle.group = toggleGroup;
            }
        }

        internal void Setup(ColorVisualAsset visualAsset)
        {
            VisualAsset = visualAsset;
            image.color = visualAsset.Color;
        }
        
        public void SetStateWithoutNotify(bool state)
        {
            toggle.SetIsOnWithoutNotify(state);
        }
    }
}