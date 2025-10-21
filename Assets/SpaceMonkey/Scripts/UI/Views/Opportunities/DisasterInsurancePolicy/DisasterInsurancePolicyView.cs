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
        [SerializeField] private Button cancelButton;
        
        [SerializeField] private TextMeshProUGUI customerNameText;
        [SerializeField] private TextMeshProUGUI weekText;
        [SerializeField] private TextMeshProUGUI paymentText;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            cancelButton.OnClickAsObservable().Subscribe(_ => Controller.OnCancel()).AddTo(this);

            infoButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(true)).AddTo(this);


            infoCloseButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(false)).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}