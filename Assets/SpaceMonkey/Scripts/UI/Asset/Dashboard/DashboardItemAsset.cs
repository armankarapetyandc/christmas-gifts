using Services.AssetDatabaseService;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.Dashboard
{
    [CreateAssetMenu(fileName = "DashboardItem", menuName = "Space Monkey/Resources/DashboardItem", order = 0)]
    public class DashboardItemAsset : ScriptableAsset
    {
        [SerializeField] private Sprite dashboardItemSprite;
        [SerializeField] private string dashboardName;

        public Sprite DashboardItemSprite => dashboardItemSprite;
        public string DashboardName => dashboardName;
    }
}