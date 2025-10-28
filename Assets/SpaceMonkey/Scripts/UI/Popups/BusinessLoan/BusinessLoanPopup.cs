using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLoan
{
    public class BusinessLoanPopup : PopupPresenterWithController<BusinessLoanPopupController>
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
                Controller.RedirectToBusinessLoanStatement();
            }).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}