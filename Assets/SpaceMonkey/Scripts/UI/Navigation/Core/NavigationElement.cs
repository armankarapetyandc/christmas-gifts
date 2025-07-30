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
        [SerializeField] protected Toggle toggle;

        public TNavigationType Type => type;
        public Observable<TNavigationType> OnSelectObservable =>
            toggle.OnValueChangedAsObservable().Where(isOn => isOn).Select(_ => type);

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