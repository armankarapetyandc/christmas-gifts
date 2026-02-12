using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCardInfo
{
    public class CreditCardInfoPopup : PopupPresenterWithController<CreditCardInfoPopupController>
    {
        [SerializeField] private Button closeButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.sfx_ClickSmall);
                Controller.ClosePopup();
            }).AddTo(this);
           
            return UniTask.CompletedTask;
        }
    }
}