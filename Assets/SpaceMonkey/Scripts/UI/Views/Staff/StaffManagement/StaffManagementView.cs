using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement
{
    public class StaffManagementView : BasePresenterWithController<StaffManagementController>
    {
        public class Data : IPresenterData
        {
            public Configs.Staff Staff { get; internal set; }
            public EmployeeProfession Profession { get; internal set; }
        }

        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI playerCapacityText;
        [SerializeField] private TextMeshProUGUI workEthicText;
        [SerializeField] private TextMeshProUGUI traitText;
        [SerializeField] private Button hireButton;
        [SerializeField] private StaffItem staffItem;
        [SerializeField] private HeadCharactersScrollComponent headCharactersScrollComponent;
        [SerializeField] private Image hiredImage;

        [Inject] private AccountService _accountService;
        [Inject] private GameConfig _gameConfig;

        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            hiredImage.gameObject.SetActive(false);
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack(_data.Profession)).AddTo(this);
            _data = data as Data;
            moneyText.text = $"${_accountService.Model.Money:F2}";
            playerCapacityText.text = $"{_accountService.Model.Money:F2} hr";
            staffItem.Set(_data.Staff, true);
            headCharactersScrollComponent
                .Setup(_gameConfig.Staffs.Where(staff =>
                    staff.Profession == _data.Profession && !CheckStaffExists(staff.Id)).ToArray())
                .Subscribe(UpdateUi)
                .AddTo(this);
            hireButton.OnClickAsObservable().Subscribe(_ => OnHireClicked()).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void OnHireClicked()
        {
            hireButton.interactable = false;
            hiredImage.gameObject.SetActive(true);
            Controller.HireStaff(_data.Staff);
        }

        private void UpdateUi(Configs.Staff staff)
        {
            _data.Staff = staff;
            staffItem.Set(_data.Staff, true);
            var exists = CheckStaffExists(staff.Id);
            hireButton.interactable = !exists;
            hiredImage.gameObject.SetActive(exists);
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