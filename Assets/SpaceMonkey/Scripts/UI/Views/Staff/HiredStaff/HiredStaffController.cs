using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Staff.HiredStaff
{
    public class HiredStaffController : BasePresenterController
    {
        private readonly AccountService _accountService;

        public HiredStaffController(PresenterService presenterService, AccountService accountService) : base(
            presenterService)
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

        public void OnFireButtonCLicked(string employeeCharacterId, EmployeeProfession employeeProfession)
        {
            OnBack(employeeProfession);
            _accountService.Model.Account.FireEmployee(employeeCharacterId);
            _accountService.SaveAsync().Forget();
        }
    }
}