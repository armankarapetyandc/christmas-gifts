using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.DisasterInsurance
{
    public class DisasterInsuranceView : BasePresenterWithController<DisasterInsuranceViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button purchasePolicyButton;
        [SerializeField] private Button learnAboutInsuranceButton;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI priceInfoText;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button infoCloseButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.sfx_ClickSmall);
                Controller.OnBack();
            }).AddTo(this);
            purchasePolicyButton.OnClickAsObservable().Subscribe(_ => Controller.PurchasePolicy()).AddTo(this);
            learnAboutInsuranceButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(true))
                .AddTo(this);
            
            infoCloseButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(false)).AddTo(this);
            moneyText.text = $"${Controller.Money}";
            priceInfoText.text = $"<B>The policy costs ${Controller.InsurancePrice}/month.</B>";
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}