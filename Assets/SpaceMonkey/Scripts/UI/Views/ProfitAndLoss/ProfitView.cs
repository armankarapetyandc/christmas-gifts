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
        public class Data : IPresenterData
        {
            public bool EnableBackButton;
            public bool UseSimulation;
            public bool IsWeekEnd;
        }
        
        
        [SerializeField] private Button infoButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private IconComponent iconComponent;
        [SerializeField] private TextMeshProUGUI weekNumberText;
        [SerializeField] private ExpensesComponent expensesComponent;
        [SerializeField] private RevenueComponent revenueComponent;
        [SerializeField] private CashComponent cashComponent;
        [SerializeField] private OverallTotalsComponent overallTotalsComponent;
        
        private Data _viewData;
        public override UniTask Initialize(IPresenterData data = null)
        {
            _viewData = data as Data;
            nextButton.OnClickAsObservable().Subscribe(_ => Controller.OnNext(_viewData.IsWeekEnd)).AddTo(this);
            backButton.OnClickAsObservable().Subscribe(_ => Controller.OnBack()).AddTo(this);
            SetupDefaults();
            return UniTask.CompletedTask;
        }
        
        private void SetupDefaults()
        {
            var account = Controller.GetAccount();
            businessName.text = account.Company.CompanyName;
            weekNumberText.text = (account.Week-1).ToString();
            var shapeVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.ShapeVisualAssetId);
            var iconVisualAsset =
                Controller.ResolveVisualAsset<SpriteVisualAsset>(account.Company.Logo.IconVisualAssetId);
            var colorVisualAsset =
                Controller.ResolveVisualAsset<ColorVisualAsset>(account.Company.Logo.BackgroundColorVisualAssetId);

            iconComponent.SetShape(shapeVisualAsset);
            iconComponent.SetIcon(iconVisualAsset);
            iconComponent.SetColor(colorVisualAsset);
            expensesComponent.Initialize(Controller.GetTotalQuantitiesByProduct(_viewData.UseSimulation));
            revenueComponent.Initialize(Controller.GetTotalQuantitiesByProduct(_viewData.UseSimulation));
            cashComponent.Initialize(Controller.GetTotalQuantitiesByProduct(_viewData.UseSimulation));
            //overallTotalsComponent.SetTotals(0f, revenueComponent.TotalCash.CurrentValue, 0f);
            var totalExpense = Controller.GetWeekTotalExpenses(_viewData.UseSimulation);
            var totalRevenue = Controller.GetWeekRevenue(_viewData.UseSimulation);
            var totalProfit = Controller.GetWeekProfit(_viewData.UseSimulation);

            overallTotalsComponent.SetTotals(totalExpense, totalRevenue, totalProfit);
            
            backButton.gameObject.SetActive(_viewData.EnableBackButton);
            nextButton.gameObject.SetActive(!_viewData.EnableBackButton);
        }

        public override void Dispose()
        {
        }
    }
}