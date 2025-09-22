using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class ManageTabComponent : MonoBehaviour
    {
        [SerializeField] private ManageItem employeeItemPrefab;
        [SerializeField] private RectTransform container;

        private readonly List<ManageItem> _employeeItems = new List<ManageItem>();
        private List<Observable<Employee>> _observables = new List<Observable<Employee>>();
        
        [Inject] private AccountService _accountService;
        [Inject] private GameConfig _gameConfig;
        
        public void Init()
        {
            foreach (var manageItem in _employeeItems)
            {
                Destroy(manageItem.gameObject);
            }

            _employeeItems.Clear();
            var employees = _accountService.Model.Account.Employees;
            foreach (var employee in employees)
            {
                var manageItem = Instantiate(employeeItemPrefab, container);
                var character = GetCharacter(employee.CharacterId);
                manageItem.Set(employee, character);
                _employeeItems.Add(manageItem);
            }
        }

        public Observable<Employee> UpdateStaffsList(EmployeeProfession selected)
        {
            _observables.Clear();
            foreach (var employee in _employeeItems)
            {
                if (selected == EmployeeProfession.All || employee.Employee.Profession == selected)
                {
                    _observables.Add(employee.OnMoreButtonClick);
                    employee.gameObject.SetActive(true);
                }
                else
                {
                    employee.gameObject.SetActive(false);
                }
            }

            return _observables.Merge();
        }
        
        private CharacterConfig GetCharacter(string selectedId)
        {
            var characters = _gameConfig.Characters;
            return characters?.FirstOrDefault(character => character.Id == selectedId);
        }
    }
}