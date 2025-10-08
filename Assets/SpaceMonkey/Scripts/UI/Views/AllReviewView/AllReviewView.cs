using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.Profile.Simulation;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using SpaceMonkey.Scripts.UI.Views.Review;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.AllReviewView
{
    public class AllReviewView : BasePresenterWithController<AllReviewController>
    {
        [SerializeField] private Button infoButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI ratingText;
        [SerializeField] private TextMeshProUGUI weekNumberText;
        [SerializeField] private ReviewItem reviewItemPrefab;
        [SerializeField] private ReviewWeekInfoItem reviewWeekInfoItemPrefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private GameObject olderReviewInfo;

        public override UniTask Initialize(IPresenterData data = null)
        {
            closeButton.OnClickAsObservable().Subscribe(_ => Controller.Close()).AddTo(this);
            InitializeInfoPanel();
            //var customers = Controller.GetReviews();
            var reviews = Controller.GetReviews();
            var weeks = Controller.GetWeeks();
            var orderedWeeks = weeks.OrderByDescending(info => info.Week).ToList();
            weekNumberText.text = weeks.Max(info => info.Week).ToString();
            var weekOrderMap = weeks
                .Select((w, index) => new {w.Id, index})
                .ToDictionary(x => x.Id, x => x.index);


            var groupedReviews = reviews
                .GroupBy(r => r.WeekId)
                .ToDictionary(g => g.Key, g => g.ToList());


            var orderedReviewsByWeek = weeks
                .Where(w => groupedReviews.ContainsKey(w.Id))
                .OrderBy(w => weekOrderMap[w.Id])
                .ToDictionary(
                    w => w.Week,
                    w => groupedReviews[w.Id]
                );


            if (reviews != null)
            {
                bool isLastWeek = true;
                bool olderReviewInfoShown = false;
                foreach (var item in orderedReviewsByWeek)
                {
                    if (!isLastWeek)
                    {
                        var reviewWeekInfoItem = Instantiate(reviewWeekInfoItemPrefab, container);
                        float review = Controller.CalculateCompanyWeekRating(orderedWeeks.Where(info => info.Week <= item.Key).ToList());
                        reviewWeekInfoItem.Initialize(item.Key, review);
                    }

                    foreach (var reviewInfo in item.Value)
                    {
                        var character = Controller.GetCharacterConfig(reviewInfo.CharacterId);
                        if (character == null)
                        {
                            continue;
                        }

                        var orderInfo = weeks.FirstOrDefault(info => info.Id == reviewInfo.WeekId).Orders
                            .FirstOrDefault(order => order.CharacterId == character.Id);


                        var reviewItem = Instantiate(reviewItemPrefab, container);
                        reviewItem.SetCharacterVisual(character.Sprite, character.BackgroundColor);
                        reviewItem.SetMood(orderInfo.Mood);
                        reviewItem.SetTextData(character.Name, reviewInfo.Message);
                        reviewItem.SetRating(Controller.GetRatingByCustomerMood(orderInfo.Mood));
                    }
                    isLastWeek = false;
                    if (!olderReviewInfoShown)
                    {
                        olderReviewInfoShown = true;
                        Instantiate(olderReviewInfo, container);
                    }
              
                }
            }

            return UniTask.CompletedTask;
        }

        private void InitializeInfoPanel()
        {
            var account = Controller.GetAccount();
            var companyRating = Controller.CalculateCompanyOngoingWeekRating();
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