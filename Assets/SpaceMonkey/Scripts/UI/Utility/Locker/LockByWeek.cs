using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public class LockByWeek : LockBy
    {
        [field: SerializeField] public float Count { get; private set; }
        [SerializeField] protected Button featureButton;

        public override void Unlock()
        {
            featureButton.interactable = true;
            icon.SetActive(false);
            PlayerPrefs.SetInt(Key, 1);
        }
    }
}