using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public class LockByWeek : LockBy
    {
        [field: SerializeField] public float Count { get; private set; }

        public override void Unlock()
        {
            featureButton.interactable = true;
            icon.SetActive(false);
            PlayerPrefs.SetInt(Key, 1);
        }
    }
}