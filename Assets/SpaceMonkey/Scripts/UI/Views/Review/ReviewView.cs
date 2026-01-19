using System.Linq;
using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile.Simulation;
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

        private Data _viewData;
        public override UniTask Initialize(IPresenterData data = null)
        {
            _viewData = data as Data;
            nextButton.OnClickAsObservable().Subscribe(_ =>
            {
                SfxPlayer.Play(Sounds.Button_Tap);
                Controller.OnNext(_viewData.IsWeekEnd);
            }).AddTo(this);
            InitializeInfoPanel();
            var simulationWeek = Controller.GetSimulationWeek();
            var reviews = Controller.GetReviews();
            weekNumberText.text = (Controller.GetAccount().Week-1).ToString();
            if (reviews != null)
            {
                foreach (CustomerReviewInfo reviewInfo in reviews)
                {
                    var customer = simulationWeek.Orders.Select(o => o.Customer)
                        .FirstOrDefault(c => c.CharacterId.Equals(reviewInfo.CharacterId));
                    if (!customer.IsValid)
                    {
                        continue;
                    }

                    var characterConfig = Controller.GetCharacterConfig(customer.CharacterId);
                    var reviewItem = Instantiate(reviewItemPrefab, container);
                    reviewItem.SetCharacterVisual(characterConfig.Sprite, characterConfig.BackgroundColor);
                    reviewItem.SetMood(customer.Mood);
                    reviewItem.SetTextData(characterConfig.Name, reviewInfo.Message);
                    reviewItem.SetRating(Controller.GetRatingByCustomerMood(customer.Mood));
                }
            }
            
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
        
        public class Data : IPresenterData
        {
            public bool IsWeekEnd;
        }

    }
}