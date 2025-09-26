using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessIconBuilder;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration;
using SpaceMonkey.Scripts.UI.Views.Business.CategorySelection;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview
{
    public class BusinessPreviewViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly GameConfig _gameConfig;
        private ScoresConfigs _scoresConfigs;

        public BusinessPreviewViewController(PresenterService presenterService, AccountService accountService,
            VisualAssetDatabase visualAssetDatabase, GameConfig gameConfig,ScoresConfigs scoresConfigs) : base(presenterService)
        {
            _scoresConfigs = scoresConfigs;
            _accountService = accountService;
            _visualAssetDatabase = visualAssetDatabase;
            _gameConfig = gameConfig;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal HashtagInfo[] GetAvailableHashtags(string category)
        {
            return _gameConfig.Categories.SingleOrDefault(info => info.Name.Equals(category))?.Tags;
        }

        internal void OnBack()
        {
            _accountService.Model.Account.Reset();
            PresenterService.HidePreviousAndShow<CategorySelectionView>().Forget();
        }

        internal void BuildLogo()
        {
            // PresenterService.HidePreviousAndShow<BusinessIconBuilderView>().Forget();
            PresenterService.Show<BusinessIconBuilderView>().Forget();
        }

        internal void SelectMoreTags()
        {
            // PresenterService.HidePreviousAndShow<BusinessHashtagsSelectionView>().Forget();
            PresenterService.Show<BusinessHashtagsSelectionView>().Forget();
        }

        internal void OnSave()
        {
            _accountService.Model.Account.Score += GetAccount().Company.Tags.Length * _scoresConfigs.CalculateScoreConfigByKey("hashtag");
            _accountService.Model.Account.Score += _scoresConfigs.CalculateScoreConfigByKey("saveLogo");
            //_accountService.Model
            _accountService.SaveAsync().Forget();
            PresenterService.HidePreviousAndShow<BusinessSetupCelebrationView>().Forget();
        }
    }
}