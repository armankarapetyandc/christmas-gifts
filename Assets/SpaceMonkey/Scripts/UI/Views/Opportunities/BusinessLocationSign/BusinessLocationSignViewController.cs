using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign
{
    public class BusinessLocationSignViewController : BasePresenterController
    {
        private readonly AccountService _accountService;
        private readonly GameConfig _gameConfig;
        public float CashAmount => _accountService.Model.Account.Money;
        public int LocationPrice => _gameConfig.NewLocationInfo.Price;
        
        public BusinessLocationSignViewController(PresenterService presenterService, 
            AccountService accountService, GameConfig gameConfig) : base(presenterService)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
        }

        public void OnSign()
        {
            var account = _accountService.Model.Account;
            if (account.CanAfford(_gameConfig.NewLocationInfo.Price))
            {
                account.Buy(_gameConfig.NewLocationInfo.Price);
                _accountService.SaveAsync().Forget();
                PresenterService.Show<BusinessLocationSplashView>().Forget();
            }
        }
        
        public void OnBack()
        {
            PresenterService.HidePreviousAndShow<OpportunitiesView>().Forget();
        }
    }
}