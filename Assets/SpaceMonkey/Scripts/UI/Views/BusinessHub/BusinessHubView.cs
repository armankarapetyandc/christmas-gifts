using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Dashboard;
using TMPro;
using UIService.Runtime.Core;
using UIService.Runtime.Presenter.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubView : BasePresenterWithController<BusinessHubController>
    {
        [SerializeField] private Image shapeImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private TextMeshProUGUI weekNumber;
        [SerializeField] private TextMeshProUGUI levelNumber;
        
        [SerializeField] private Button startButton;
        
        [SerializeField] private DashboardItem[] dashboardItems;
        
        public override UniTask Initialize(IPresenterData data = null)
        {
            shapeImage.sprite = Controller.ShapeSprite();
            iconImage.sprite = Controller.IconSprite();
            shapeImage.color = Controller.ShapeColor();
            businessName.text = Controller.GetBusinessName();
            SetupItems();
            return UniTask.CompletedTask;
        }

        private void SetupItems()
        {
            var items = Controller.RetrieveIdeas();
            var count = Mathf.Min(dashboardItems.Length, items.Count);
            for (int i = 0; i < count; i++)
            {
                var item = dashboardItems[i];
                item.Set(items[i]);
            }
            
            dashboardItems
                .Where(item => item.HasItem)
                .Select(item => item.SelectedItem)
                .Merge()
                .Subscribe(ItemSelected)
                .AddTo(this);
        }

        private void ItemSelected(DashboardItemAsset item)
        {
            Controller.ShowSelected(item.Id);
        }

        public override void Dispose()
        {
        }
    }
}