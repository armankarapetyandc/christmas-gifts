using System.Linq;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;
using SpaceMonkey.Scripts.Configs;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfo
{
    public class LevelInfoController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;
        private AccountService _accountService;
        private GameConfig _gameConfig;

        public LevelInfoController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            AccountService accountService, GameConfig gameConfig) : base(presenterService)
        {
            _gameConfig = gameConfig;
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
        }

        public int GetLevel()
        {
            return _accountService.Model.Account.Level;
        }

        public int GetScore()
        {
            return (int)_accountService.Model.Account.Score;
        }

        public Configs.LevelInfo GetLeveInfoData(int level)
        {
            Configs.LevelInfo levelInfo = _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
            return levelInfo ?? _gameConfig.LevelInfos[^1];
        }

        public new void Close()
        {
            _popupPresenterService.HideLast();
        }
    }
}