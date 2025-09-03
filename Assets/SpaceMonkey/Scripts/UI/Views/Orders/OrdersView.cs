using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using R3;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrdersView : BasePresenterWithController<OrdersViewController>
    {
        [SerializeField] private IconComponent companyIconComponent;
        [SerializeField] private TextMeshProUGUI weekText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI productionCapacityText;
        [SerializeField] private OrderItem orderItemPrefab;
        [SerializeField] private RectTransform container;

        private readonly List<OrderItem> _orders = new List<OrderItem>();

        public override UniTask Initialize(IPresenterData data = null)
        {
            Controller.GetAvailableProdCapObservable()
                .Subscribe(value => productionCapacityText.text = $"{value} hrs").AddTo(this);
            Controller.GetMoneyObservable()
                .Subscribe(value => moneyText.text = $"${value}").AddTo(this);
            SetupDefaults();
            SetupCustomers();
            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();

            var companyShapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var companyIconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var companyColorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            companyIconComponent.SetShape(companyShapeVisualAsset);
            companyIconComponent.SetIcon(companyIconVisualAsset);
            companyIconComponent.SetColor(companyColorVisualAsset);

            weekText.text = account.Week.ToString();
        }

        private void SetupCustomers()
        {
            var customers = Controller.GetSimulationCustomers();
            var moodVisualAssets = Controller.ResolveVisualAssets<MoodVisualAsset>().OrderBy(asset => asset.MoodValue)
                .ToList();
            foreach (Customer customer in customers)
            {
                var orderItem = Instantiate(orderItemPrefab, container);
                orderItem.SetCustomer(customer);
                orderItem.ShipOrder.Subscribe(item => ShipOrder(item).Forget()).AddTo(this);
                orderItem.SetCharacterVisual(customer.Character.Sprite, customer.Character.BackgroundColor);
                var moodAsset = moodVisualAssets.FirstOrDefault(asset => customer.Mood <= asset.MoodValue);
                orderItem.SetMood(customer.Mood, moodAsset);

                var products = customer.Orders.Select(o => (
                    productOrder: o,
                    iconVisualAsset: Controller.ResolveVisualAsset<SpriteVisualAsset>(o.Product.IconVisualAssetId),
                    colorVisualAsset:
                    Controller.ResolveVisualAsset<ColorVisualAsset>(o.Product.BackgroundColorVisualAssetId)
                )).ToList();

                orderItem.SetProducts(products);
                _orders.Add(orderItem);
            }
        }

        private async UniTaskVoid ShipOrder(OrderItem item)
        {
            var isShipped = await Controller.TryShipOrder(item.Customer);
            if (!isShipped)
            {
                Controller.FinishWeek();
                return;
            }

            _orders.Remove(item);
            Destroy(item.gameObject);

            if (_orders.Count==0)
            {
                Controller.FinishWeek();
            }
        }

        public override void Dispose()
        {
        }
    }
}