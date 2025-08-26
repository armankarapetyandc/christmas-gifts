using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Asset.Database;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ProfitController : BasePresenterController
    {
        private readonly VisualAssetDatabase _visualAssetDatabase;
        private readonly AccountService _accountService;

        public ProfitController(PresenterService presenterService, VisualAssetDatabase visualAssetDatabase,
            AccountService accountService) : base(presenterService)
        {
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

        public void OnNext()
        {
            
        }
    }
}