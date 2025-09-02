using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI staffName;
        [SerializeField] private Button moreButton;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI payrollText;
        [SerializeField] private TextMeshProUGUI capacityText;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Slider experienceSlider;

        public Configs.Staff Staff { get; private set; }

        public void Set(Configs.Staff staff)
        {   
            Staff = staff;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if(Staff == null) return;
            staffName.text = Staff.StaffName;
            payrollText.text = $"-${Staff.PayrollText}/week";
            capacityText.text = $"+{Staff.CapacityText} hrs/week";
            experienceSlider.value = Staff.Experience;
            speedSlider.value = Staff.Speed;
            SetIcon(Staff.Character.Sprite);
        }

        private void SetIcon(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                iconImage.sprite = asset.Sprite;
            }

            iconImage.gameObject.SetActive(asset != null);
        }
        
    }
}