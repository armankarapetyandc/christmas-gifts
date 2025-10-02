using System;
using SpaceMonkey.Scripts.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class UnlockedItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image icon;

        private LevelUnlockInfo _LevelUnlockInfo;

        public void Initialize(LevelUnlockInfo levelUnlockInfo)
        {
            _LevelUnlockInfo = levelUnlockInfo;
            gameObject.SetActive(true);
            icon.sprite = _LevelUnlockInfo.Icon;
            description.text = _LevelUnlockInfo.Description;
        }
    }
}