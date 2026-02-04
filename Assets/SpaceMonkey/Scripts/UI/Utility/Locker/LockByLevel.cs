using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public class LockByLevel : LockBy
    {
        public bool Locked { get; private set; } = true;

        public override void Unlock()
        {
            PlayerPrefs.SetInt(Key,1);
            Locked = false;
            icon.SetActive(false);
        }
    }
}