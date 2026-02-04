using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public class LockByMoney : LockBy
    {
        [field: SerializeField] public float Value { get; private set; }
        [SerializeField] protected Button featureButton;
        public override void Unlock()
        {
            PlayerPrefs.SetInt(Key,1);
            featureButton.interactable = true;
            icon.SetActive(false);
        }
    }
}