using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubView : BasePresenterWithController<BusinessHubController>
    {
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private TextMeshProUGUI weekNumber;
        [SerializeField] private TextMeshProUGUI levelNumber;

        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI prodCapText;
        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private Button startButton;
        [SerializeField] private IconComponent productComponent;
        [SerializeField] private TextMeshProUGUI productsCountText;

        [SerializeField] private Button productionButton;

        public override UniTask Initialize(IPresenterData data = null)
        {
            productComponent.OnClick.Subscribe(_ => Controller.ShowProductView()).AddTo(this);
            startButton.OnClickAsObservable().Subscribe(_ => Controller.StartWeek()).AddTo(this);
            productionButton.OnClickAsObservable().Subscribe(_ => Controller.ShowProductionView()).AddTo(this);
            SetupDefaults();
            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            businessName.text = account.Company.CompanyName;
            levelNumber.text = $"Level {account.Level.ToString()}";
            moneyText.text = $"${account.Money.ToString()}";
            prodCapText.text = $"{account.GetProductionCapacity()} hrs";
            scoreText.text = $"${account.Score.ToString()}";
            weekNumber.text = account.Week.ToString();
            productsCountText.text = account.Products.Count == 0
                ? "Products"
                : $"Products({account.Products.Count.ToString()})";

            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);

            PreviewProductAtIndexIfExists(0);
        }

        private void PreviewProductAtIndexIfExists(int index)
        {
            var account = Controller.GetAccount();
            if (account.Products == null || account.Products.Count == 0 || index >= account.Products.Count)
            {
                return;
            }

            var product = account.Products[index];

            var iconVisualAsset = Controller.ResolveVisualAsset<SpriteVisualAsset>(product.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(product.BackgroundColorVisualAssetId);

            productComponent.SetIcon(iconVisualAsset);
            productComponent.SetColor(colorVisualAsset);
        }

        public override void Dispose()
        {
        }
    }
}