using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingView : BasePresenterWithController<MarketingController>
    {
        [SerializeField] private Button infoButton;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI totalCostText;
        [SerializeField] private TextMeshProUGUI weekNumberText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [field: SerializeField] private List<MarketingItem> Items { get; set; }
        
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}