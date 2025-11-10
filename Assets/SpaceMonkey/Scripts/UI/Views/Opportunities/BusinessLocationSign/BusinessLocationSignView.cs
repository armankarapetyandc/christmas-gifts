using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign
{
    public class BusinessLocationSignView : BasePresenterWithController<BusinessLocationSignViewController>
    {
        [SerializeField] private Button signButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI cashAmountText;
        [SerializeField] private TextMeshProUGUI locationPriceText;
        private Data _data;

        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = data as Data;
            signButton.interactable = Controller.CashAmount >= Controller.LocationPrice;
            signButton.OnClickAsObservable().Subscribe(_ => Controller.OnSign(_data.PlaceId)).AddTo(this);
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            cashAmountText.text = $"${Controller.CashAmount}";
            locationPriceText.text = $"${Controller.LocationPrice}";
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            
        }
        
        public class Data : IPresenterData
        {
            public int PlaceId;
        }
    }
}