using System;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Components;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

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
        public async void HireStaff(Configs.Staff hireStaff,Transform transform)
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
            var score= _scoresConfigs.CalculateScoreConfigByKey("HireStaff");
            if (score > 0)
            {
                XPParticleEffector.SpawnXpParticles(score, new Vector2(Screen.width, Screen.height) * 0.5f, transform)
                    .Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            }
            _accountService.Model.Account.Score += score;
            _accountService.SaveAsync().Forget();
        }
    }
    
}