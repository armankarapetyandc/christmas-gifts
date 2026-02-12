using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsurancePolicy
{
    public class DisasterInsurancePolicyView : BasePresenterWithController<DisasterInsurancePolicyViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button infoCloseButton;
        [SerializeField] private Button bottomInfoCloseButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private RectTransform bottomInfoContainer;
        [SerializeField] private TextMeshProUGUI customerNameText;
        [SerializeField] private TextMeshProUGUI weekText;
        [SerializeField] private TextMeshProUGUI paymentText;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.sfx_ClickSmall);
                Controller.OnBack();
            }).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ => Controller.OnCancel()).AddTo(this);

            infoButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(true)).AddTo(this);


            infoCloseButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(false)).AddTo(this);
            bottomInfoCloseButton.OnClickAsObservable().Subscribe(_ =>
            {
                PlayerPrefs.SetInt("DisasterInsurancePolicyViewInfoShown", 1);
                bottomInfoContainer.gameObject.SetActive(!PlayerPrefs.HasKey("DisasterInsurancePolicyViewInfoShown"));
            }).AddTo(this);
            weekText.text = $"Week {Controller.Week}";
            paymentText.text = $"${Controller.InsurancePrice}";
            bottomInfoContainer.gameObject.SetActive(!PlayerPrefs.HasKey("DisasterInsurancePolicyViewInfoShown"));
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}