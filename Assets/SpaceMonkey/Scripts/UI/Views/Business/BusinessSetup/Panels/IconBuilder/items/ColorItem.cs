using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.IconBuilder.items
{
    public class ColorItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Toggle toggle;

        public Observable<Color> OnSelected =>
            toggle.OnValueChangedAsObservable()
                .Where(isOn => isOn)
                .Select(_ => iconImage.color);

        internal void Set(Color color)
        {
            iconImage.color = color;
        }
    }
}