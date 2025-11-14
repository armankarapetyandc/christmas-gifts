using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder;
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
        [SerializeField] private BigOrderItem[] orderItems;

        [SerializeField] private Button cancelButton;

        protected override void InternalInit()
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack(PresenterData.Type)).AddTo(this);
            infoButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(true)).AddTo(this);

            cancelButton.OnClickAsObservable().Subscribe(_ => Controller.OnCancel(PresenterData.Type)).AddTo(this);

            infoCloseButton.OnClickAsObservable().Subscribe(_ => infoPanel.SetActive(false)).AddTo(this);
            SetupDefaults();
        }

        private void SetupDefaults()
        {
            var orderEntries = Controller.GetOrders();
            for (int i = 0; i < orderEntries.Count; i++)
            {
                var item = orderItems[i];
                var order = orderEntries[i];
                var iconVisualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(order.Product.IconVisualAssetId);
                var colorVisualAsset =
                    Controller.ResolveVisualAsset<ColorVisualAsset>(order.Product.BackgroundColorVisualAssetId);
                item.Set(iconVisualAsset, colorVisualAsset);
                item.SetAmount(order.Quantity);
                item.gameObject.SetActive(true);
            }
            var totalProfit = orderEntries.Sum(x => x.OrderProfit);
            var totalProdCost = orderEntries.Sum(x => x.OrderProdCost);
            var ordersDone = orderEntries.Count(x => x.Ship);
            moneyText.text = $"+${totalProfit:F2}";
            productCostText.text = $"-{Mathf.RoundToInt(totalProdCost)}hrs";
            bigOrderRatioText.text = ordersDone.ToString();
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