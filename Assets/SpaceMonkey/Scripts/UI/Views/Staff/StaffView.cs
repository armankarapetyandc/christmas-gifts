using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffView : BasePresenterWithController<StaffController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private StaffItem staffItemPrefab;
        [SerializeField] private RectTransform container;
        
        private StaffItem[] _staffItems;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            var staffs = Controller.GetStaffs();
            foreach (var staff in staffs)
            {
                var staffItem = Instantiate(staffItemPrefab, container);
                staffItem.Set(staff);
            }
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}