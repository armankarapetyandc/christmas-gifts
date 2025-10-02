using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement
{
    public class StaffManagementController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private ScoresConfigs _scoresConfigs;

        public StaffManagementController(PresenterService presenterService, AccountService accountService,ScoresConfigs scoresConfigs) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _accountService = accountService;
        }

        public void OnBack(EmployeeProfession employeeProfession)
        {
            PresenterService.Show<StaffView>(new StaffView.Data()
            {
                Profession = employeeProfession
            }).Forget();
        }

        public Account GetAccount()
        {
            return _accountService.Model.Account;
        }
        public void HireStaff(Configs.Staff hireStaff)
        {
            Employee newEmployee = new Employee()
            {
                Profession = hireStaff.Profession,
                Payroll = hireStaff.Payroll,
                Capacity = hireStaff.Capacity,
                Speed = hireStaff.Speed,
                Experience = hireStaff.Experience,
                CharacterId = hireStaff.Character.Id,
                Id = hireStaff.Id
            };
            _accountService.Model.Account.SetEmployee(newEmployee);
            _accountService.Model.Account.Score+= _scoresConfigs.CalculateScoreConfigByKey("HireStaff");
            _accountService.SaveAsync();
        }
    }
    
}