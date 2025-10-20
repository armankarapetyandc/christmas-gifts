using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderView : BasePresenterWithController<BigOrderViewController>
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button infoButton;

        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productCostText;
        [SerializeField] private TextMeshProUGUI donutText;
        [SerializeField] private TextMeshProUGUI iceText;
        [SerializeField] private TextMeshProUGUI gingerText;

        [SerializeField] private Button acceptButton;
        [SerializeField] private Button declineButton;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => Controller.OnInfo()).AddTo(this);
            
            acceptButton.OnClickAsObservable().Subscribe(_ => Controller.OnAccept()).AddTo(this);
            declineButton.OnClickAsObservable().Subscribe(_ => Controller.OnDecline()).AddTo(this);
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            
        }
    }
}