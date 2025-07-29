using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class BackgroundItem : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image selectedBackground;
        
        public Observable<Color> OnSelectedColor =>
            toggle.OnValueChangedAsObservable()
                .Where(isOn => isOn) 
                .Select(_ => backgroundImage.color);

        internal void Set(Color color)
        {
            backgroundImage.color = color;
        }
    }
}