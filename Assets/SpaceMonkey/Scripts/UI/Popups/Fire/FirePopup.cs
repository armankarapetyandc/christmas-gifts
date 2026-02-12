using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using TMPro;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.Fire
{
    public class FirePopup : PopupPresenterWithController<FirePopupController>
    {
        [SerializeField] private Button viewCapacityButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI currentProductionText;

        public override UniTask Initialize(IPresenterData data = null)
        {
            currentProductionText.text =
                $"Current Production Capacity: {Controller.GetProductionCapacity()}(-{Controller.GetCapacityReductionPercent()}%)";
            viewCapacityButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                Controller.NavigateToProductionCapacity();
            }).AddTo(this);

            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.sfx_ClickSmall);
                Controller.ClosePopUp();
            }).AddTo(this);

            return UniTask.CompletedTask;
        }
    }
}