using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
        private float _totalCost = 0f;

        [Inject] private AccountService _accountService;

        public override UniTask Initialize(IPresenterData data = null)
        {
            var account = Controller.GetAccount();
            Debug.LogError(account.GetMarketingCustAdd());
            backButton.OnClickAsObservable().Subscribe(_ => OnBack()).AddTo(this);
            moneyText.text = $"${account.Money:f2}";
            weekNumberText.text = $"Week {account.Week.ToString()}";
            totalCostText.text = "$0.00";
            var marketingInfos = Controller.GetMarketingInfos();
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                var marketingInfo = marketingInfos[i];
                var index = _accountService.Model.Account.MarketingFeatures.FindIndex(f => f.Id == marketingInfo.Id);

                item.Initialize(marketingInfo, _accountService.Model.Account.Level,
                    index == -1 ? new MarketingFeature()  : _accountService.Model.Account.MarketingFeatures[index]);
            }

            Items.Select(item => item.MarketingSlider.CurrentValue.Select(_ => Unit.Default)).Merge().Subscribe(_ =>
            {
                var selected = Items.Where(item => item.IsSelected).ToArray();
                _totalCost = 0;
                foreach (var item in selected)
                {
                    _totalCost += item.MarketingSlider.CurrentValue.CurrentValue;
                }

                totalCostText.text = $"${_totalCost:f2}";
            });


            return UniTask.CompletedTask;
        }

        private void OnBack()
        {
            var marketingFeatures = (from item in Items
                where item.IsSelected
                select new MarketingFeature()
                {
                    Id = item.MarketingItemInfo.Id,
                    MinMult = item.MarketingItemInfo.MinMult,
                    MaxMult = item.MarketingItemInfo.MaxMult,
                    Division = item.MarketingItemInfo.Div,
                    CurrentPrice = item.MarketingSlider.CurrentValue.CurrentValue,
                    Unlock = item.MarketingItemInfo.Unlock,
                }).ToList();
            Controller.UpdateMarketingFeatures(marketingFeatures);
        }

        public override void Dispose()
        {
        }
    }
}