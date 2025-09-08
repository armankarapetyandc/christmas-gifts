using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class HireTabComponent : MonoBehaviour
    {
        [SerializeField] private StaffItem staffItemPrefab;
        [SerializeField] private RectTransform container;

        private readonly List<StaffItem> _staffItems = new List<StaffItem>();
        private List<Observable<Configs.Staff>> _observables = new List<Observable<Configs.Staff>>();

        [Inject] private GameConfig _gameConfig;
        [Inject] private AccountService _accountService;

        private void Start()
        {
            foreach (var staff in _staffItems)
            {
                Destroy(staff.gameObject);
            }

            _staffItems.Clear();
            var staffs = _gameConfig.Staffs;
            foreach (var staff in staffs)
            {
                if (CheckStaffExists(staff.Character.Id)) continue;
                var staffItem = Instantiate(staffItemPrefab, container);
                staffItem.Set(staff);
                _staffItems.Add(staffItem);
            }
        }

        public Observable<Configs.Staff> UpdateStaffsList(EmployeeProfession selected)
        {
             _observables.Clear();
            foreach (var staff in _staffItems)
            {
                if (selected == EmployeeProfession.All || staff.Staff.Profession == selected)
                {
                    _observables.Add(staff.OnMoreButtonClick);
                    staff.gameObject.SetActive(true);
                }
                else
                {
                    staff.gameObject.SetActive(false);
                }
                
            }
            return _observables.Merge();
        }

        private bool CheckStaffExists(string staffId)
        {
            return _accountService.Model.Account.Employees.Any(employee => employee.CharacterId == staffId);
        }
    }
}