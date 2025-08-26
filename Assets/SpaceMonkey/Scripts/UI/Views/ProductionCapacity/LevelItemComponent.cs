using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class LevelItemComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private Image levelIcon;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image lockImage;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image alertImage;

        private LevelVisualAsset _visualAsset;
        private Profile.LevelProdCap _level;
        
        public Color BackgroundColor => _visualAsset.BackgroundColor;

        public Observable<Profile.LevelProdCap> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b && _visualAsset).Select(_ => _level);

        private void Start()
        {
            if (transform.parent.TryGetComponent<ToggleGroup>(out var toggleGroup))
            {
                toggle.group = toggleGroup;
            }
        }

        public void ChangeState()
        {
            toggle.isOn = !toggle.isOn;
        }

        internal void Setup(LevelVisualAsset visualAsset, Profile.LevelProdCap levelInfo, int index, bool isLocked)
        {
            _visualAsset = visualAsset;
            _level = levelInfo;
            levelIcon.sprite = visualAsset.LevelIconSprite;
            levelIndexText.text = (index + 1).ToString();
            levelText.text = $"Level {index + 1}";
            lockImage.gameObject.SetActive(isLocked);
        }

        public void UpdateLevelUi()
        {
            lockImage.gameObject.SetActive(false);
        }

        public void UpdateLevelIconSprite(bool isBroken)
        {
            levelIcon.sprite = isBroken ? _visualAsset.BrokenLevelIconSprite : _visualAsset.LevelIconSprite;
            levelIndexText.text = isBroken ? "Broken" : levelIndexText.text;
            alertImage.gameObject.SetActive(isBroken);
        }
    }
}