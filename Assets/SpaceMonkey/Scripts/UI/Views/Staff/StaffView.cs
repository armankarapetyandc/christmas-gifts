using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffView : BasePresenterWithController<StaffController>
    {
        public class  Data : IPresenterData
        {
            public EmployeeProfession Profession { get; internal set; }
        }
        
        [SerializeField] private Button backButton;
        [SerializeField] private StaffProfessionTab[] staffTabs;
        [SerializeField] private HireTabComponent hireTabComponent;
        [SerializeField] private ManageTabComponent manageTabComponent;
        
        private Observable<Configs.Staff> _staff;
        private Observable<Employee> _employee;
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            staffTabs
                .Select(tab => tab.OnSelected)
                .Merge()
                .Subscribe(selected =>
                {
                    _staff = hireTabComponent.UpdateStaffsList(selected);
                    _staff?.Subscribe(employee => Controller.OnStaffSelected(employee, selected));
                })
                .AddTo(this);
            
            staffTabs
                .Select(tab => tab.OnSelected)
                .Merge()
                .Subscribe(selected =>
                {
                    _employee =  manageTabComponent.UpdateStaffsList(selected);
                    _employee?.Subscribe(employee => Controller.OnEmployeeSelected(employee, selected));
                })
                .AddTo(this);
            
            staffTabs
                .Where(staff => staff.EmployeeProfession == _data.Profession)
                .ToList()
                .ForEach(staff => staff.toggle.isOn = true);
            
            return UniTask.CompletedTask;
        }
        
        public override void Dispose()
        {
        }
    }
}