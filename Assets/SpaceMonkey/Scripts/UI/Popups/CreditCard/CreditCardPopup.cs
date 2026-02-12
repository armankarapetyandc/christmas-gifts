using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCard
{
    public class CreditCardPopup : PopupPresenterWithController<CreditCardPopupController>
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button confirmButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.sfx_ClickSmall);
                Controller.Close();
            }).AddTo(this);
            confirmButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                Controller.RedirectToCreditCardStatement();
                Controller.Close();
            }).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}