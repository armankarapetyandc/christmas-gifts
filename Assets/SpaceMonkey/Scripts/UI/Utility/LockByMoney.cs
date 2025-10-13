using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility
{
    public class LockByMoney : MonoBehaviour
    {
        [field: SerializeField] public float Value { get; private set; }
        [SerializeField] private Button featureButton;
        [SerializeField] private GameObject icon;
        
        public void Unlock()
        {
            featureButton.interactable = true;
            icon.SetActive(false);
        }

    }
}