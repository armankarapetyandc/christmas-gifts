using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails
{
    public class BigOrderDetailsView : BasePresenterWithController<BigOrderDetailsView.Data,BigOrderDetailsViewController>
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
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button infoCloseButton;

        [SerializeField] private Button cancelButton;

        protected override void InternalInit()
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(true)).AddTo(this);

            cancelButton.OnClickAsObservable().Subscribe(_ => Controller.OnCancel(PresenterData.Type)).AddTo(this);

            infoCloseButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(false)).AddTo(this);
        }



        public override void Dispose()
        {
        }
        
        public class Data : IPresenterData
        {
            public MainNavigationType Type { get; set; }
        }
    }
}