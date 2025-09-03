using SpaceMonkey.Scripts.UI.Asset.Database;
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
        [SerializeField] private Image[] speed;
        [SerializeField] private Image[] experience;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Sprite filledStar; 

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
            SetRating(Staff.Speed, speed);
            SetRating(Staff.Experience, experience);
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
        
        private void SetRating(int rating, Image[]  stars)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].sprite = i < rating ? filledStar : emptyStar;
            }
        }
        
    }
}