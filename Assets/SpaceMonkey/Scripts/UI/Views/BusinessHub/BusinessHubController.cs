using System.Collections.Generic;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Dashboard;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubController : BasePresenterController
    {
        private readonly IconBuilderConfig _iconBuilderConfig;
        private readonly AccountService _accountService;
        private readonly DashboardAssetDatabase _dashboardAssetDatabase;

        public BusinessHubController(PresenterService presenterService, IconBuilderConfig iconBuilderConfig,
            AccountService accountService, DashboardAssetDatabase dashboardAssetDatabase) : base(presenterService)
        {
            _iconBuilderConfig = iconBuilderConfig;
            _accountService = accountService;
            _dashboardAssetDatabase = dashboardAssetDatabase;
        }
        
        internal List<DashboardItemAsset> RetrieveIdeas()
        {
            return _dashboardAssetDatabase.Assets;
        }
        
        public Sprite ShapeSprite()
        {
            return _iconBuilderConfig.GetShapeSprite(_accountService.Account.Company.Logo.Shape);
        }

        public Sprite IconSprite()
        {
            return _iconBuilderConfig.GetIconSprite(_accountService.Account.Company.Logo.Icon);
        }

        public Color ShapeColor()
        {
            return ColorUtility.TryParseHtmlString("#" + _accountService.Account.Company.Logo.Background, out var color)
                ? color
                : Color.white;
        }
        
        public string GetBusinessName()
        {
            return _accountService.Account.Company.CompanyName;
        }
    }
}