using System.Linq;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto
{
    public class LevelInfoAutoController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;
        private GameConfig _gameConfig;

        public LevelInfoAutoController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            GameConfig gameConfig) : base(presenterService)
        {
            _gameConfig = gameConfig;
            _popupPresenterService = popupPresenterService;
        }

        public Configs.LevelInfo GetLeveInfoData(int level)
        {
            return _gameConfig.LevelInfos.FirstOrDefault(info => info.Level == level);
        }
    }
}