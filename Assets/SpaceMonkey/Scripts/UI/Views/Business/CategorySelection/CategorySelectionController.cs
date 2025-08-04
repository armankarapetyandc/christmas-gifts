using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Services.AssetDatabaseService;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset;
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
        private readonly GameConfig _gameConfig;

        public CategorySelectionController(PresenterService presenterService, AccountService accountService,GameConfig gameConfig) : base(presenterService)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
        }

        internal CategoryInfo[] RetrieveIdeas()
        {
            return _gameConfig.Categories;
        }

        internal void Back()
        {
            PresenterService.HidePreviousAndShow<StartupView>().Forget();
        }

        internal void OnNext()
        {
            PresenterService.HidePreviousAndShow<BusinessSetupView>().Forget();
        }

        internal void IdeaSelected(string category)
        {
            _accountService.Model.Account.SetCategory(category);
        }
    }
}