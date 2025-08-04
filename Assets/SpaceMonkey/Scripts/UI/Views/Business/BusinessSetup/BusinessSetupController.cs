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

        internal void OnBack()
        {
            PresenterService.HidePreviousAndShow<CategorySelectionView>().Forget();
        }

        internal void OnNext()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupCelebrationView>().Forget();
        }

        internal void SetCompanyLogoData(Sprite shapeSprite, Sprite iconSprite, Color backgroundColor)
        {
            _accountService.Model.Account.SetCompanyLogo(shapeSprite.name, iconSprite.name,
                ColorUtility.ToHtmlStringRGBA(backgroundColor));
        }

        internal void SetCompanyHashTags(Hashtag[] tags)
        {
            _accountService.Model.Account.SetTags(tags);
        }

        internal string GetCurrentCategory() => _accountService.Model.Account.Company.Category;
    }
}