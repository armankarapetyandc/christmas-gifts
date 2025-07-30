using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder.items
{
    public class IconItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Toggle toggle;
        
        public Observable<Sprite> OnSelected =>
            toggle.OnValueChangedAsObservable()
                .Where(isOn => isOn) 
                .Select(_ => icon.sprite);

        internal void Set(Sprite sprite)
        {
            if (sprite == null) return;
            icon.sprite = sprite;
        }
    }
}
