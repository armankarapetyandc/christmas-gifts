using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility.Locker
{
    public abstract class LockBy : MonoBehaviour
    {
        [field: SerializeField] public string Key { get; private set; }
        [SerializeField] protected GameObject icon;

        public abstract void Unlock();
    }
}