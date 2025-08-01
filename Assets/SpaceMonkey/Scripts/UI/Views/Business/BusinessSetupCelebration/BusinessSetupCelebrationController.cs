using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration
{
    public class BusinessSetupCelebrationController : BasePresenterController
    {
        private readonly IconBuilderConfig _iconBuilderConfig;
        private readonly AccountService _accountService;
        private readonly NavigationPresenterService _navigationPresenterService;

        public BusinessSetupCelebrationController(PresenterService presenterService,
            IconBuilderConfig iconBuilderConfig, AccountService accountService,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _iconBuilderConfig = iconBuilderConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
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

        public void OnBack()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupView>().Forget();
        }

        public void OnNext()
        {
            PresenterService.Hide();
            _navigationPresenterService.Show<MainNavigation>();

            _accountService.SaveAsync();
            //Debug.LogError(Application.persistentDataPath);
        }

        public string GetBusinessName()
        {
            return _accountService.Account.Company.CompanyName;
        }
    }
}