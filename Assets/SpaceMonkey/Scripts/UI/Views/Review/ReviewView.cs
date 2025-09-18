using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Review
{
    public class ReviewView : BasePresenterWithController<ReviewController>
    {
        [SerializeField] private Button infoButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI ratingText;
        [SerializeField] private TextMeshProUGUI weekNumberText;
        [SerializeField] private ReviewItem reviewItemPrefab;
        [SerializeField] private RectTransform container;

        public override UniTask Initialize(IPresenterData data = null)
        {
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            InitializeInfoPanel();
            var customers = Controller.GetSimulationCustomers();
            var reviewItem = Instantiate(reviewItemPrefab, container);
            reviewItem.SetCharacterVisual(customers[0].Character.Sprite,customers[0].Character.BackgroundColor);
            reviewItem.SetMood(50f);
            return UniTask.CompletedTask;
        }

        private void InitializeInfoPanel()
        {
            var account = Controller.GetAccount();
            var companyRating = Controller.CalculateCompanyRating();
            businessName.text = account.Company.CompanyName;
            ratingText.text = $"{companyRating:F1}";

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

        public override void Dispose()
        {
        }
    }
}