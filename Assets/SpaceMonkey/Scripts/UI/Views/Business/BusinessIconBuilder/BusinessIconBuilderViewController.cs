using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessIconBuilder
{
    public class BusinessIconBuilderViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        internal readonly CompanyLogo CompanyLogo = new CompanyLogo();

        public BusinessIconBuilderViewController(PresenterService presenterService, AccountService accountService,
            VisualAssetDatabase visualAssetDatabase) : base(presenterService)
        {
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal IEnumerable<T> ResolveVisualAssets<T>(Predicate<T> predicate = null) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourcesForAsset(predicate);
        }

        internal void OnBack()
        {
            PresenterService.HidePreviousAndShow<BusinessPreviewView>().Forget();
        }

        internal void OnSave()
        {
            _accountService.Model.Account.SetCompanyLogo(CompanyLogo);
            OnBack();
        }
    }
}