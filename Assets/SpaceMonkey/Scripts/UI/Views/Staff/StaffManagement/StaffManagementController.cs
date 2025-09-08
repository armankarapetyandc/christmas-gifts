using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement
{
    public class StaffManagementController : BasePresenterController
    {
        private readonly AccountService _accountService;

        public StaffManagementController(PresenterService presenterService, AccountService accountService) : base(presenterService)
        {
            _accountService = accountService;
        }

        public void OnBack(EmployeeProfession employeeProfession)
        {
            PresenterService.Show<StaffView>(new StaffView.Data()
            {
                Profession = employeeProfession
            }).Forget();
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
                CharacterId = hireStaff.Character.Id
            };
            _accountService.Model.Account.SetEmployee(newEmployee);
            _accountService.SaveAsync();
        }
    }
    
}