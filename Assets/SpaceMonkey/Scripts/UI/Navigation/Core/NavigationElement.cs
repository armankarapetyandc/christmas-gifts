using System;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Navigation.Core
{
    public class NavigationElement<TNavigationType> : MonoBehaviour where TNavigationType : Enum
    {
        [SerializeField] private TNavigationType type;
        [SerializeField] private Toggle toggle;

        public TNavigationType Type => type;
        public Observable<TNavigationType> OnSelectObservable => toggle.OnSelectAsObservable().Select(_ => type);

        public void Select()
        {
            toggle.isOn = true;
            toggle.Select();
        }

        public void SelectWithoutNotification()
        {
            toggle.SetIsOnWithoutNotify(true);
        }
    }
}