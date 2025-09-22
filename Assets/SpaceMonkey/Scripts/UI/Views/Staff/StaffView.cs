using System;
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
        private IDisposable _staffSubscription;
        private Observable<Employee> _employee;
        private IDisposable _employeeSubscription;
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            
            manageTabComponent.Init();
            hireTabComponent.Init();
            
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            staffTabs
                .Select(tab => tab.OnSelected)
                .Merge()
                .Subscribe(InitStuff)
                .AddTo(this);
            
            staffTabs
                .Select(tab => tab.OnSelected)
                .Merge()
                .Subscribe(InitEmployee)
                .AddTo(this);
            
            staffTabs
                .Where(staff => staff.EmployeeProfession == _data.Profession)
                .ToList()
                .ForEach(staff => staff.toggle.isOn = true);
            
            // InitEmployee(_data.Profession);
            // InitStuff(_data.Profession);
            return UniTask.CompletedTask;
        }

        private void InitEmployee(EmployeeProfession selected)
        {
            _employee =  manageTabComponent.UpdateStaffsList(selected);
            _employeeSubscription?.Dispose();
            _employeeSubscription = _employee?.Subscribe(employee => Controller.OnEmployeeSelected(employee, selected));
        }

        private void InitStuff(EmployeeProfession selected)
        {
            _staff = hireTabComponent.UpdateStaffsList(selected);
            _staffSubscription?.Dispose();
            _staffSubscription = _staff?.Subscribe(employee => Controller.OnStaffSelected(employee, selected));
        }

        public override void Dispose()
        {
        }
    }
}