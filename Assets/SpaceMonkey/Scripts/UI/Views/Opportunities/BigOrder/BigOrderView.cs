using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderView : BasePresenterWithController<BigOrderView.Data,BigOrderViewController>
    {
        [SerializeField] private Button backButton;

        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productCostText;
        [SerializeField] private TextMeshProUGUI donutText;
        [SerializeField] private TextMeshProUGUI iceText;
        [SerializeField] private TextMeshProUGUI gingerText;

        [SerializeField] private Button acceptButton;
        [SerializeField] private Button declineButton;
        
        protected override void InternalInit()
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack(PresenterData.Type)).AddTo(this);

            
            acceptButton.OnClickAsObservable().Subscribe(_ => Controller.OnAccept(PresenterData.Type)).AddTo(this);
            declineButton.OnClickAsObservable().Subscribe(_ => Controller.OnDecline(PresenterData.Type)).AddTo(this);
            
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