using System.Linq;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.DemoComplete;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto
{
    public class LevelInfoAutoController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;
        private AccountService _accountService;
        private GameConfig _gameConfig;

        public int Level => _accountService.Model.Account.Level;
        public int Score => (int) _accountService.Model.Account.Score;

        public LevelInfoAutoController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            AccountService accountService, GameConfig gameConfig) : base(presenterService)
        {
            _gameConfig = gameConfig;
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
        }

        public Configs.LevelInfo GetLeveInfoData(int level)
        {
            Configs.LevelInfo levelInfo = _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
            return levelInfo ?? _gameConfig.LevelInfos[^1];
        }
        
        public new void Close()
        {
            _popupPresenterService.HideLast();
            if (Level == 5)
            {
                PresenterService.Show<DemoCompleteView>();
            }

        }
    }
}