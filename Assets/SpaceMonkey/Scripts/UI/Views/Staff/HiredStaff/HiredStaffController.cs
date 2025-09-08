using Cysharp.Threading.Tasks;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Staff.HiredStaff
{
    public class HiredStaffController : BasePresenterController
    {
        public HiredStaffController(PresenterService presenterService) : base(presenterService)
        {
        }

        public void OnBack(EmployeeProfession employeeProfession)
        {
            PresenterService.Show<StaffView>(new StaffView.Data()
            {
                Profession = employeeProfession
            }).Forget();
        }
    }
}