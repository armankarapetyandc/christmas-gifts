using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Services.AssetDatabaseService;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.BusinessIdeas;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup;
using SpaceMonkey.Scripts.UI.Views.Startup;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class CategorySelectionController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly BusinessIdeaAssetDatabase _ideaAssetDatabase;

        public CategorySelectionController(PresenterService presenterService, AccountService accountService,
            BusinessIdeaAssetDatabase ideaAssetDatabase) : base(presenterService)
        {
            _accountService = accountService;
            _ideaAssetDatabase = ideaAssetDatabase;
        }

        // internal List<IdeaInfo> RetrieveIdeas()
        // {
        //     return _ideaAssetDatabase.Assets.Select(asset =>
        //         new IdeaInfo(asset.BusinessIdeaName, asset.BusinessIdeaItemAsset)).ToList();
        // }

        internal List<BusinessIdeaAsset> RetrieveIdeas()
        {
            return _ideaAssetDatabase.Assets;
        }

        public void Back()
        {
            PresenterService.HidePreviousAndShow<StartupView>().Forget();
        }

        public void OnNext()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupView>().Forget();
        }
        
        public void IdeaSelected(string ideaName, string ideaId)
        {
            _accountService.Account.SetCategory(ideaName, ideaId);
        }
    }
}