using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

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


        public override UniTask Initialize(IPresenterData data = null)
        {
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
            moneyText.text = $"${account.Money:C}";
            productionCapacityText.text = $"${account.GetProductionCapacity()} hrs";
        }
        private void SetupCustomers()
        {
            var customers = Controller.GetSimulationCustomers();
            var moodVisualAssets = Controller.ResolveVisualAssets<MoodVisualAsset>().OrderBy(asset => asset.MoodValue)
                .ToList();
            foreach (Customer customer in customers)
            {
                var orderItem = Instantiate(orderItemPrefab, container);
                orderItem.SetCustomerName(customer.Character.Name);
                orderItem.SetCharacterVisual(customer.Character.Sprite,customer.Character.BackgroundColor);
                Debug.LogError(customer.Mood);
                var moodAsset = moodVisualAssets.FirstOrDefault(asset => customer.Mood <= asset.MoodValue);
                orderItem.SetMood(customer.Mood, moodAsset);
            }
        }
        public override void Dispose()
        {
        }
    }
}