using System.Linq;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class ManageItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI staffName;
        [SerializeField] private TextMeshProUGUI professionText;
        [SerializeField] private Button moreButton;
        [SerializeField] private TextMeshProUGUI payrollText;
        [SerializeField] private TextMeshProUGUI capacityText;
        [SerializeField] private Image[] speed;
        [SerializeField] private Image[] experience;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Sprite filledStar;

        private bool _isFullBody = false;
        private CharacterConfig _character;
         
        public Employee Employee { get; private set; }

        public Observable<Employee> OnMoreButtonClick => moreButton.OnClickAsObservable().Select(_ => Employee);

        public void Set(Employee staff,CharacterConfig character, bool isFullBody = false)
        {
            Employee = staff;
            _isFullBody = isFullBody;
            _character = character;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (Employee == null) return;
            if (_character == null) return;
            staffName.text = _character.Name;
            professionText.text = Employee.Profession.ToString();
            payrollText.text = $"-${Employee.Payroll}/week";
            capacityText.text = $"+{Employee.Capacity} hrs/week";
            SetRating(Employee.Speed, speed);
            SetRating(Employee.Experience, experience);
            SetIcon(_isFullBody ? _character.FullBodySprite : _character.Sprite);
        }

        private void SetIcon(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                iconImage.sprite = asset.Sprite;
            }

            iconImage.gameObject.SetActive(asset != null);
        }

        private void SetRating(int rating, Image[] stars)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].sprite = i < rating
                    ? filledStar
                    : _isFullBody
                        ? null
                        : emptyStar;
            }
        }
    }
}