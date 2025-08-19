using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class LevelItemComponent :MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private Image levelIcon;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image lockImage;
        
        private ProductionLevelInfo _levelInfo;

        private LevelVisualAsset _visualAsset;

        internal void Setup(LevelVisualAsset visualAsset, ProductionLevelInfo levelInfo)
        {
            _visualAsset = visualAsset;
            levelIcon.sprite = visualAsset.LevelIconSprite;
            SetLock(levelInfo.Index != 0);
            levelIndexText.text = levelInfo.Index.ToString();
            levelText.text = $"Level {levelInfo.Index}";
        }

        public void SetLock(bool locked)
        {
            lockImage.gameObject.SetActive(locked);
        }

        public void UpdateLevelIconSprite(bool isBroken)
        {
            levelIcon.sprite = isBroken ? _visualAsset.BrokenLevelIconSprite : _visualAsset.LevelIconSprite;
        }

    }
}