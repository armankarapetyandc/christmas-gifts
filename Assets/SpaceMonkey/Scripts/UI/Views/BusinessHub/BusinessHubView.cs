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
        [SerializeField] private Button productButton;
        [SerializeField] private TextMeshProUGUI productsCountText;

        public override UniTask Initialize(IPresenterData data = null)
        {
            productButton.OnClickAsObservable().Subscribe(_ => Controller.ShowProductView()).AddTo(this);
            SetupDefaults();
            return UniTask.CompletedTask;
        }

        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            businessName.text = account.Company.CompanyName;
            levelNumber.text = $"Level {account.Level.ToString()}";
            moneyText.text = $"${account.Money.ToString()}";
            prodCapText.text = $"{account.ProductionCapacity} hrs";
            scoreText.text = $"${account.Score.ToString()}";
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
        }

        // private void SelectFirstProduct()
        // {
        //     if (_accountService.Model.Account.Products.Count > 0)
        //     {
        //         firstProductIconImage.color = Color.white;
        //         firstProductBackgroundImage.color = Color.white;
        //         var firstProd = _accountService.Model.Account.Products[0];
        //         // firstProductBackgroundImage.color = BackgroundColor(firstProd.BackgroundColor);
        //         // firstProductIconImage.sprite = Icon(firstProd.IconVisualAssetId);
        //     }
        //     else
        //     {
        //         firstProductBackgroundImage.color = diselectedColor;
        //         firstProductIconImage.sprite = null;
        //         firstProductIconImage.color = diselectedColor;
        //     }
        // }

        public override void Dispose()
        {
        }
    }
}