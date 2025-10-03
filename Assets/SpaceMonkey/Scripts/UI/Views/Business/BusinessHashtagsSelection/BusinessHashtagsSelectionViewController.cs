using System.Linq;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection
{
    public class BusinessHashtagsSelectionViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly ScoresConfigs _scoresConfigs;
        private readonly GameConfig _gameConfig;

        public BusinessHashtagsSelectionViewController(PresenterService presenterService, AccountService accountService,ScoresConfigs scoresConfigs,
            GameConfig gameConfig) : base(presenterService)
        {
            _accountService = accountService;
            _scoresConfigs = scoresConfigs;
            _gameConfig = gameConfig;
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal HashtagInfo[] GetAvailableHashtags(string category)
        {
            return _gameConfig.Categories.SingleOrDefault(info => info.Name.Equals(category))?.Tags;
        }

        internal HashtagInfo[] GetSelectedHashtags()
        {
            var account = GetAccount();
            return GetAvailableHashtags(account.Company.Category)
                .Where(info => account.Company.Tags.Any(tag => tag.Tag.Equals(info.Tag)))
                .ToArray();
        }

        internal void Save(Hashtag[] tags)
        {
            _accountService.Model.Account.SetTags(tags);
            OnBack();
        }

        internal void OnBack()
        {
            PresenterService.HidePreviousAndShow<BusinessPreviewView>().Forget();
        }
        internal int GetScoreFor(string key)
        {
            return _scoresConfigs.PeekScoreConfigByKey(key);
        }
    }
}