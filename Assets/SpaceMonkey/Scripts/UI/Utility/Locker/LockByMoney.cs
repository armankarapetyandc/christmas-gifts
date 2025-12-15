using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public class LockByMoney : LockBy
    {
        [field: SerializeField] public float Value { get; private set; }

        public override void Unlock()
        {
            featureButton.interactable = true;
            icon.SetActive(false);
            PlayerPrefs.SetInt(Key, 1);
        }
    }
}