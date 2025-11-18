using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.Fire
{
    public class FirePopup : PopupPresenterWithController<FirePopupController>
    {
        [SerializeField] private Button viewCapacityButton;
        [SerializeField] private Button closeButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            viewCapacityButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.NavigateToProductionCapacity();
            }).AddTo(this);
            
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.ClosePopUp();
            }).AddTo(this);
            
            return UniTask.CompletedTask;
        }
    }
}
