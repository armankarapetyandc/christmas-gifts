using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Product;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubController : BasePresenterController
    {
        private readonly IconBuilderConfig _iconBuilderConfig;
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService  _navigationPresenterService;

        public BusinessHubController(PresenterService presenterService, IconBuilderConfig iconBuilderConfig,
            AccountService accountService, NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _iconBuilderConfig = iconBuilderConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
        }

        
        public Sprite ShapeSprite()
        {
            return _iconBuilderConfig.GetShapeSprite(_accountService.Model.Account.Company.Logo.Shape);
        }

        public Sprite IconSprite()
        {
            return _iconBuilderConfig.GetIconSprite(_accountService.Model.Account.Company.Logo.Icon);
        }

        public Color ShapeColor()
        {
            return ColorUtility.TryParseHtmlString("#" + _accountService.Model.Account.Company.Logo.Background, out var color)
                ? color
                : Color.white;
        }
        
        public string GetBusinessName()
        {
            return _accountService.Model.Account.Company.CompanyName;
        }

        public void ShowProductView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.Show<ProductCreationView>();
        }
    }
}