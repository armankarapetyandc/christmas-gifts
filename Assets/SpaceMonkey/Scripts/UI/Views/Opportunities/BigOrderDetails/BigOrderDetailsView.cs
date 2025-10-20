using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails
{
    public class BigOrderDetailsView : BasePresenterWithController<BigOrderDetailsViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productCostText;
        [SerializeField] private TextMeshProUGUI donutText;
        [SerializeField] private TextMeshProUGUI iceText;
        [SerializeField] private TextMeshProUGUI gingerText;
        [SerializeField] private TextMeshProUGUI customerNameText;
        [SerializeField] private TextMeshProUGUI bigOrderRatioText;

        [SerializeField] private Button cancelButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => Controller.OnInfo()).AddTo(this);

            cancelButton.OnClickAsObservable().Subscribe(_ => Controller.OnCancel()).AddTo(this);
            return UniTask.CompletedTask;
        }



        public override void Dispose()
        {
        }
    }
}