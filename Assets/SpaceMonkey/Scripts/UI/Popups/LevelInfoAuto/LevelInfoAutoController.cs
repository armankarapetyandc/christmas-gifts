using System.Linq;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Views.DemoComplete;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto
{
    public class LevelInfoAutoController : BasePresenterController
    {
        private PopupPresenterService _popupPresenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private AccountService _accountService;
        private readonly NavigationPresenterService navigationPresenterService;
        private GameConfig _gameConfig;

        public int Level => _accountService.Model.Account.Level;
        public int Score => (int) _accountService.Model.Account.Score;

        public LevelInfoAutoController(PresenterService presenterService, NavigationPresenterService navigationPresenterService,PopupPresenterService popupPresenterService,VisualAssetDatabase visualAssetDatabase,
            AccountService accountService, GameConfig gameConfig) : base(presenterService)
        {
            this.navigationPresenterService = navigationPresenterService;
            _gameConfig = gameConfig;
            _accountService = accountService;
            _popupPresenterService = popupPresenterService;
            _visualAssetDatabase = visualAssetDatabase;
        }

        public Configs.LevelInfo GetLeveInfoData(int level)
        {
            return _gameConfig.GetLeveInfoData(level);
        }
        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }
        public new void Close()
        {
            _popupPresenterService.HideLast();
            if (Level >= _gameConfig.LevelInfos.Length)
            {
                navigationPresenterService.HideAll();
                PresenterService.Show<DemoCompleteView>();
            }
        }
    }
}