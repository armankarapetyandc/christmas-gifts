using R3;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Navigation.Bottom
{
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class MainNavigationElement : NavigationElement<MainNavigationType>
    {
        [SerializeField] private Image selectedImage;

        private void Start()
        {
            toggle.OnValueChangedAsObservable()
                .Subscribe(state => { selectedImage.gameObject.SetActive(state); }).AddTo(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            selectedImage.gameObject.SetActive(toggle.isOn);
        }

        private void LateUpdate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            selectedImage.gameObject.SetActive(toggle.isOn);
        }
#endif
    }
}