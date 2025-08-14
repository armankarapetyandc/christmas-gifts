using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Product.ProductList;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService  _navigationPresenterService;

        public BusinessHubController(PresenterService presenterService,
            AccountService accountService, NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
        }

        
        public Sprite ShapeSprite()
        {
            // return _iconBuilderConfig.GetShapeSprite(_accountService.Model.Account.Company.Logo.Shape);
            return Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 0, 0),Vector2.zero);
        }

        public Sprite IconSprite()
        {
            // return _iconBuilderConfig.GetIconSprite(_accountService.Model.Account.Company.Logo.Icon);
            return Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, 0, 0),Vector2.zero);
        }

        public Color ShapeColor()
        {
            // return ColorUtility.TryParseHtmlString("#" + _accountService.Model.Account.Company.Logo.Background, out var color)
            //     ? color
            //     : Color.white;
            return Color.white;
        }
        
        public string GetBusinessName()
        {
            return _accountService.Model.Account.Company.CompanyName;
        }

        public void ShowProductView()
        {
            _navigationPresenterService.HideAll();
            PresenterService.HidePreviousAndShow<ProductListView>().Forget();
        }
    }
}