using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Components;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ProfitView : BasePresenterWithController<ProfitController>
    {
        [SerializeField] private Button infoButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI weekNumberText;
        [SerializeField] private ExpensesComponent expensesComponent;
        [SerializeField] private RevenueComponent revenueComponent;
        [SerializeField] private CashComponent cashComponent;
        [SerializeField] private OverallTotalsComponent overallTotalsComponent;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext()).AddTo(this);
            SetupDefaults();
            return UniTask.CompletedTask;
        }
        
        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            businessName.text = account.Company.CompanyName;
            weekNumberText.text = account.Week.ToString();
            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
            expensesComponent.Initialize(Controller.GetTotalQuantitiesByProduct());
            revenueComponent.Initialize(Controller.GetTotalQuantitiesByProduct());
            //overallTotalsComponent.SetTotals(0f, revenueComponent.TotalCash.CurrentValue, 0f);
            var totalExpense = Controller.GetWeekTotalExpenses();
            var totalRevenue = Controller.GetWeekRevenue();
            var totalProfit = Controller.GetWeekProfit();

            overallTotalsComponent.SetTotals(totalExpense, totalRevenue, totalProfit);
        }

        public override void Dispose()
        {
        }
    }
}