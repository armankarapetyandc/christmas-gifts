using System.Linq;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.UpgradeCapacity
{
    public class UpgradeCapacityController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private GameConfig _gameConfig;

        public int Score => (int)_accountService.Model.Account.Score;
        public int Level => _accountService.Model.Account.Level;

        public UpgradeCapacityController(PresenterService presenterService, AccountService accountService,
            GameConfig gameConfig) : base(presenterService)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
        }

        public LevelInfo GetLeveInfoData(int level)
        {
            LevelInfo levelInfo = _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
            return levelInfo ?? _gameConfig.LevelInfos[^1];
        }
    }
}