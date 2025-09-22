using R3;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement
{
    public class CharacterHeadItem : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image characterImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite selectedStateSprite;
        [SerializeField] private Sprite deselectedStateSprite;

        private Configs.Staff _staff;
        private Employee _employee;

        public Observable<Configs.Staff> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b).Select(_ => _staff);

        public Observable<Employee> OnSelectedEmployee =>
            toggle.OnValueChangedAsObservable().Where(b => b).Select(_ => _employee);

        public void Set(Configs.Staff staff, ToggleGroup toggleGroup)
        {
            _staff = staff;
            UpdateUI();
            SetToggleGroup(toggleGroup);
        }
        
        public void SetEmployee(Employee employee,CharacterConfig character ,ToggleGroup toggleGroup)
        {
            _employee = employee;
            UpdateEmployeeUi(character);
            SetToggleGroup(toggleGroup);
        }

        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(OnStateChanged).AddTo(this);
        }

        private void OnStateChanged(bool state)
        {
            backgroundImage.sprite = state ? selectedStateSprite : deselectedStateSprite;
        }

        private void UpdateUI()
        {
            if (_staff == null) return;
            SetIcon(_staff.Character.Sprite);
        }

        private void UpdateEmployeeUi(CharacterConfig character)
        {
            if(_employee == null) return;
            SetIcon(character.Sprite);
        }

        private void SetToggleGroup(ToggleGroup group)
        {
            toggle.group = group;
        }

        private void SetIcon(SpriteVisualAsset asset)
        {
            if (asset != null)
            {
                characterImage.sprite = asset.Sprite;
            }

            characterImage.gameObject.SetActive(asset != null);
        }

        public void SelectForce(bool state)
        {
            toggle.isOn = state;
        }
    }
}