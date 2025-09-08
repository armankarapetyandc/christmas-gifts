using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff.HiredStaff
{
    public class HiredStaffView : BasePresenterWithController<HiredStaffController>
    {
        public class Data : IPresenterData
        {
            public Employee Employee { get; internal set; }
            public EmployeeProfession Profession { get; internal set; }
        }

        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI playerCapacityText;
        [SerializeField] private TextMeshProUGUI workEthicText;
        [SerializeField] private TextMeshProUGUI traitText;
        [SerializeField] private Button trainButton;
        [SerializeField] private Button benefitsButton;
        [SerializeField] private Button fireButton;
        [SerializeField] private ManageItem manageItem;
        [SerializeField] private HeadCharactersScrollComponent headCharactersScrollComponent;
        [SerializeField] private TextMeshProUGUI emotionText;

        [Inject] private AccountService _accountService;
        [Inject] private GameConfig _gameConfig;

        private Data _data;
        private CharacterConfig _characterConfig;


        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack(_data.Profession)).AddTo(this);
            _data = data as Data;
            if (_data == null) return default;
            moneyText.text = $"${_accountService.Model.Money:F2}";
            playerCapacityText.text = $"{_accountService.Model.Money:F2} hr";
            _characterConfig = GetCharacter(_data.Employee.CharacterId);
            manageItem.Set(_data.Employee, _characterConfig, true);
            headCharactersScrollComponent
                .Setup(_accountService.Model.Account.Employees.Where(employee =>
                    employee.Profession == _data.Profession && CheckStaffExists(employee.Id)).ToArray())
                .Subscribe(UpdateUi)
                .AddTo(this);
            fireButton.OnClickAsObservable().Subscribe(_ =>
                    Controller.OnFireButtonCLicked(_data.Employee.Id, _data.Profession))
                .AddTo(this);
            return UniTask.CompletedTask;
        }


        private void UpdateUi(Employee employee)
        {
            _data.Employee = employee;
            _characterConfig = GetCharacter(_data.Employee.CharacterId);
            manageItem.Set(_data.Employee, _characterConfig, true);
        }

        private CharacterConfig GetCharacter(string selectedId)
        {
            var characters = _gameConfig.Characters;
            return characters?.FirstOrDefault(character => character.Id == selectedId);
        }

        private bool CheckStaffExists(string staffId)
        {
            return _accountService.Model.Account.Employees.Any(employee => employee.CharacterId == staffId);
        }

        public override void Dispose()
        {
        }
    }
}