using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffView : BasePresenterWithController<StaffController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private StaffItem staffItemPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private StaffProfessionTab[] staffTabs;

        private List<StaffItem> _staffItems = new List<StaffItem>();

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            staffTabs
                .Select(tab => tab.OnSelected)
                .Merge()
                .Subscribe(UpdateStaffsList)
                .AddTo(this);

            return UniTask.CompletedTask;
        }

        private void UpdateStaffsList(StaffProfessionEnum selected)
        {
            foreach (var staff in _staffItems)
            {
                Destroy(staff.gameObject);
            }

            _staffItems.Clear();
            var staffs = Controller.GetStaffs();
            
            foreach (var staff in staffs)
            {
                if (staff.Profession != selected) continue;
                var staffItem = Instantiate(staffItemPrefab, container);
                staffItem.Set(staff);
                _staffItems.Add(staffItem);
            }
        }

        public override void Dispose()
        {
        }
    }
}