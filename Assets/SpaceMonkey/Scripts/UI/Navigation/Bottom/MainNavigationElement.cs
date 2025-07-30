using System;
using R3;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Navigation.Bottom
{
    public class MainNavigationElement : NavigationElement<MainNavigationType>
    {
        [SerializeField] private Image selectedImage;

        private void Start()
        {
            toggle.OnValueChangedAsObservable()
                .Subscribe(state => { selectedImage.gameObject.SetActive(state); }).AddTo(this);
        }
    }
}