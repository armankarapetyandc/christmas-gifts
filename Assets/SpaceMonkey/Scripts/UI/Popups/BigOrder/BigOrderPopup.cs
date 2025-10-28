using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.BigOrder
{
    public class BigOrderPopup : PopupPresenterWithController<BigOrderPopupController>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button confirmButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.Close();
            }).AddTo(this);
            confirmButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.Close();
                Controller.OpenBigOrderView();
            }).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}