using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset.Database;
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
        }

        public void SetVisualAsset(SpriteVisualAsset visualAsset)
        {
            icon.sprite = visualAsset?.Sprite;
        }
    }
}