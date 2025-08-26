using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ProfitController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly AccountService _accountService;

        public ProfitController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, VisualAssetDatabase visualAssetDatabase,
            AccountService accountService) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _visualAssetDatabase = visualAssetDatabase;
            _accountService = accountService;
        }

        internal T ResolveVisualAsset<T>(string id) where T : VisualAsset
        {
            return _visualAssetDatabase.GetResourceForAsset<T>(id);
        }

        internal Account GetAccount()
        {
            return _accountService.Model.Account;
        }

        internal void OnNext()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.BusinessHub
            }).Forget();
        }
    }
}