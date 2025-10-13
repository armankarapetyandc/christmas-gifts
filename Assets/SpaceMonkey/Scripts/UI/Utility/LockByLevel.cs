using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility
{
    public class LockByLevel : MonoBehaviour
    {
        [field: SerializeField] public string Key { get; private set; }
        [SerializeField] private Button featureButton;
        [SerializeField] private GameObject icon;

        public void Unlock()
        {
            featureButton.interactable = true;
            icon.SetActive(false);
        }
    }
}