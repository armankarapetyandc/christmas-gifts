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
                .Subscribe(value => productionCapacityText.text = $"{Mathf.RoundToInt(value)} hrs").AddTo(this);
            Controller.GetMoneyObservable()
                .Subscribe(value => moneyText.text = $"+${value:F2}").AddTo(this);
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
            var currentWeek = Controller.GetCurrentWeek();
            var moodVisualAssets = Controller.ResolveVisualAssets<MoodVisualAsset>().OrderBy(asset => asset.MoodValue)
                .ToList();
            foreach (var order in currentWeek.Orders)
            {
                var character = Controller.GetCharacter(order.Customer.CharacterId);
                var orderItem = Instantiate(orderItemPrefab, container);
                orderItem.SetOrder(order);
                orderItem.SetCustomer(character);
                orderItem.ShipOrder.Subscribe(item => ShipOrder(item).Forget()).AddTo(this);
                orderItem.SetCharacterVisual(character.Sprite, character.BackgroundColor);
                var moodAsset = moodVisualAssets.FirstOrDefault(asset => order.Customer.Mood <= asset.MoodValue);
                orderItem.SetMood(order.Customer.Mood, moodAsset);
                
                var products = order.OrderEntries.Select(o => (
                    productOrder: o,
                    iconVisualAsset: Controller.ResolveVisualAsset<SpriteVisualAsset>(o.Product.IconVisualAssetId),
                    colorVisualAsset:
                    Controller.ResolveVisualAsset<ColorVisualAsset>(o.Product.BackgroundColorVisualAssetId)
                )).ToList();
                orderItem.SetProducts(products);
                _orders.Add(orderItem);
            }

            var bigOrder = Controller.GetBigOrder();
            if (bigOrder != null)
            {
                var character = Controller.GetCharacter(bigOrder.CharacterId);
                var orderItem = Instantiate(orderItemPrefab, container);
                var order = new WeekSimulationV2.Order
                {
                    Customer = new WeekSimulationV2.CustomerData()
                    {
                        CharacterId = bigOrder.CharacterId,
                        Active = true,
                        Mood = 60,
                    },
                    OrderEntries = bigOrder.OrderEntries,
                    WasFulfilled = bigOrder.OrderEntries.All(e => e.Ship),
                    IsBigOrder = true
                };
                orderItem.SetOrder(order);
                orderItem.SetCustomer(character);
                orderItem.ShipOrder.Subscribe(item => ShipOrder(item).Forget()).AddTo(this);
                orderItem.SetCharacterVisual(character.Sprite, character.BackgroundColor);
                var moodAsset = moodVisualAssets.FirstOrDefault(asset => order.Customer.Mood <= asset.MoodValue);
                orderItem.SetMood(order.Customer.Mood, moodAsset);
                var products = order.OrderEntries.Select(o => (
                    productOrder: o,
                    iconVisualAsset: Controller.ResolveVisualAsset<SpriteVisualAsset>(o.Product.IconVisualAssetId),
                    colorVisualAsset:
                    Controller.ResolveVisualAsset<ColorVisualAsset>(o.Product.BackgroundColorVisualAssetId)
                )).ToList();
                orderItem.SetProducts(products);
                orderItem.transform.SetAsFirstSibling();
                _orders.Insert(0, orderItem);
            }
        }

        private async UniTaskVoid ShipOrder(OrderItem item)
        {
            var isShipped = await Controller.TryShipOrder(item.Order);
            if (!isShipped)
            {
                Controller.FinishWeek();
                return;
            }

            _orders.Remove(item);

            var score = Controller.GetScoreFor("sellProduct") * item.Order.OrderEntries.Sum(order => order.Quantity);
            XPParticleEffector.SpawnXpParticles(score, Input.mousePosition, transform).Forget();
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