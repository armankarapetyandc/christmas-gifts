using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup
{
    public class BusinessSetupController : BasePresenterController
    {
        private readonly AccountService _accountService;

        public BusinessSetupController(PresenterService presenterService,AccountService accountService) : base(presenterService)
        {
            _accountService = accountService;
        }

        public void OnBack()
        {
            PresenterService.HidePreviousAndShow<CategorySelectionView>().Forget();
        }

        public void OnNext()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupCelebrationView>().Forget();
        }

        public void SetCompanyLogoData(Sprite shapeSprite, Sprite iconSprite, Color backgroundColor)
        {
            _accountService.Account.SetCompanyLogo(shapeSprite.name, iconSprite.name,
                ColorUtility.ToHtmlStringRGBA(backgroundColor));
        }

        public void SetCompanyHashTags(string[] tags)
        {
            _accountService.Account.SetTags(tags);
        }
    }
}