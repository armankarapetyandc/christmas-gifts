using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLoanInfo
{
    public class BusinessLoanInfoPopup : PopupPresenterWithController<BusinessLoanInfoPopupController>
    {
        [SerializeField] private Button closeButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.ClosePopup();
            }).AddTo(this);
           
            return UniTask.CompletedTask;
        }
    }
}