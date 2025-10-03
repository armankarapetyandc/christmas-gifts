using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Scores;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
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
        private readonly ScoresConfigs _scoresConfigs;

        public CategorySelectionController(PresenterService presenterService, AccountService accountService,ScoresConfigs scoresConfigs,
            GameConfig gameConfig) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _accountService = accountService;
            _scoresConfigs = scoresConfigs;
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
            PresenterService.HidePreviousAndShow<BusinessPreviewView>().Forget();
        }

        internal void IdeaSelected(string category)
        {
            _accountService.Model.Account.Score += _scoresConfigs.CalculateScoreConfigByKey("category");
            _accountService.Model.Account.SetCategory(category);
        }

        internal int GetScoreFor(string key)
        {
            return _scoresConfigs.PeekScoreConfigByKey(key);
        }
    }
}