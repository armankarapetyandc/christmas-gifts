using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Analytics;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
using SpaceMonkey.Scripts.UI.Views.BusinessExamples;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration
{
    public class BusinessSetupCelebrationController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly NavigationPresenterService _navigationPresenterService;

        public BusinessSetupCelebrationController(PresenterService presenterService, AccountService accountService,
            VisualAssetDatabase visualAssetDatabase,
            NavigationPresenterService navigationPresenterService) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
            _navigationPresenterService = navigationPresenterService;
        }

        internal void OnBack()
        {
            PresenterService.HidePreviousAndShow<BusinessPreviewView>().Forget();
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal void OnNext()
        {
            PresenterService.Hide();
            _navigationPresenterService.Show<MainNavigation>().Forget();
            AnalyticsProvider.SendEvent(AnalyticsEvents.NewBusinessCreated,new Dictionary<string, string>()
            {
                {"company_name",_accountService.Model.Account.Company.CompanyName},
                {"company_category",_accountService.Model.Account.Company.Category},
                {"company_category_tags",string.Join(",",_accountService.Model.Account.Company.Tags.Select(hashtag => hashtag.Tag))}
            });
        }

        public void OpenExamplesView()
        {
            PresenterService.Show<BusinessExamplesView>(new BusinessExamplesView.Data
            {
                Category = _accountService.Model.Account.Company.Category
            });
        }
    }
}