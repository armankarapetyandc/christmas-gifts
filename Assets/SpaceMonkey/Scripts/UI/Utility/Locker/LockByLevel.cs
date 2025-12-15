using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public class LockByLevel : LockBy
    {
        public bool Locked { get; private set; } = true;

        public override void Unlock()
        {
            Locked = false;
            icon.SetActive(false);
        }
    }
}