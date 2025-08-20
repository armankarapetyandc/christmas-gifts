using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment
{
    public class UpgradeEquipmentPopup : PopupPresenterWithController<UpgradeEquipmentController>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI availableCashText;
        [SerializeField] private Button useCashButton;
        [SerializeField] private Button useCreditButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var account = Controller.GetAccount();
            availableCashText.text = $"${account.Money}";
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.OnClose()).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}