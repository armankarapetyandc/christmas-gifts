using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Staff.HiredStaff;
using SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly AccountService _accountService;
        [Inject] private GameConfig _gameConfig;
        
        public StaffController(PresenterService presenterService, NavigationPresenterService navigationPresenterService, AccountService accountService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _accountService = accountService;
        }
        

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }

        internal void OnStaffSelected(Configs.Staff selectedStaff, EmployeeProfession selectedProfession)
        {
            PresenterService.Show<StaffManagementView>(new StaffManagementView.Data
            {
                Staff = selectedStaff,
                Profession = selectedProfession
            }).Forget();
        }

        public void OnEmployeeSelected(Employee employee, EmployeeProfession currentProfession)
        {
            PresenterService.Show<HiredStaffView>(new HiredStaffView.Data
            {
                Employee = employee,
                Profession = currentProfession
            }).Forget();
        }
    }
}