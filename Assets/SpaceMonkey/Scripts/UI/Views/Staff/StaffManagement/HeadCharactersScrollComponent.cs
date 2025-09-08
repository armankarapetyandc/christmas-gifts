using System.Collections.Generic;
using System.Linq;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Characters;
using SpaceMonkey.Scripts.Profile;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement
{
    public class HeadCharactersScrollComponent : MonoBehaviour
    {
        [SerializeField] private CharacterHeadItem headItemPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private ToggleGroup group;

        [Inject] private GameConfig _gameConfig;

        private List<CharacterHeadItem> _headItems = new List<CharacterHeadItem>();
        
        public Observable<Configs.Staff> Setup(Configs.Staff[] staffs)
        {
            while (_headItems.Count > 0)
            {
                Destroy(_headItems[0].gameObject);
                _headItems.RemoveAt(0);
            }
            
            var observables = new List<Observable<Configs.Staff>>();
            foreach (var staff in staffs)
            {
                var item = Instantiate(headItemPrefab, container);
                item.Set(staff, group);
                _headItems.Add(item);
                observables.Add(item.OnSelected);
                
            }
            return observables.Merge();
        }
        
        public Observable<Employee> Setup(Employee[] employees)
        {
            while (_headItems.Count > 0)
            {
                Destroy(_headItems[0].gameObject);
                _headItems.RemoveAt(0);
            }
            
            var observables = new List<Observable<Employee>>();
            foreach (var employee in employees)
            {
                var item = Instantiate(headItemPrefab, container);
                item.SetEmployee(employee, GetCharacter(employee.CharacterId), group);
                _headItems.Add(item);
                observables.Add(item.OnSelectedEmployee);
            }
            return observables.Merge();
        }
        
        private CharacterConfig GetCharacter(string selectedId)
        {
            var characters = _gameConfig.Characters;
            return characters?.FirstOrDefault(character => character.Id == selectedId);
        }
    }
}