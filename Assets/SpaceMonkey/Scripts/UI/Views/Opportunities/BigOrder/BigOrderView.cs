using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
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

        [SerializeField] private BigOrderItem[] orderItems;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button declineButton;
        
        protected override void InternalInit()
        {
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack(PresenterData.Type)).AddTo(this);
            acceptButton.OnClickAsObservable().Subscribe(_ => Controller.OnAccept(PresenterData.Type)).AddTo(this);
            declineButton.OnClickAsObservable().Subscribe(_ => Controller.OnDecline(PresenterData.Type)).AddTo(this);
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
            moneyText.text = $"+${totalProfit:F2}";
            productCostText.text = $"-{Mathf.RoundToInt(totalProdCost)}hrs";
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