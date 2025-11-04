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

        public void Initialize(LevelUnlockInfo levelUnlockInfo)
        {
            gameObject.SetActive(true);
            description.text = levelUnlockInfo?.Description;
            icon.sprite = levelUnlockInfo?.IconVisualAsset?.Sprite;
        }
    }
}