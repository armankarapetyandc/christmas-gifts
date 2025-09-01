using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Extensions;
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
        private float _totalCost = 0f;


        public override UniTask Initialize(IPresenterData data = null)
        {
            var account = Controller.GetAccount();
            backButton.OnClickAsObservable().Subscribe(_ => OnBack()).AddTo(this);
            moneyText.text = $"${account.Money:f2}";
            weekNumberText.text = $"Week {account.Week.ToString()}";
            totalCostText.text = "$0.00";
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                var values = Constants.MarketingSliderInit[i];
                item.MarketingSlider.Setup(values.Item1, values.Item2, null);
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
            var marketingFeatures = new List<MarketingFeature>();
            for (var i = 0; i < Items.Count; i++)
            {
                if (!Items[i].IsSelected) continue;
                var prices = Constants.MarketingSliderInit[i];
                var marketingFeature = new MarketingFeature()
                {
                    MinPrice = prices.Item1,
                    MaxPrice = prices.Item2,
                    CurrentPrice = Items[i].MarketingSlider.CurrentValue.CurrentValue,
                };
                marketingFeatures.Add(marketingFeature);
            }
            Controller.UpdateMarketingFeatures(marketingFeatures);
        }

        public override void Dispose()
        {
        }
    }
}