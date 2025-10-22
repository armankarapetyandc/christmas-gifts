using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities.InsuranceCanceled;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsurancePolicy
{
    public class DisasterInsurancePolicyViewController : BasePresenterController
    {
        private NavigationPresenterService _navigationPresenterService;
        
        private AccountService _accountService;
        private GameConfig _gameConfig;
 
        
        public int Week => _accountService.Model.Account.Week;
        public int InsurancePrice => _gameConfig.InsuranceInfo.InsurancePrice;

        public DisasterInsurancePolicyViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService,AccountService accountService,GameConfig gameConfig) : base(presenterService)
        {
            _accountService = accountService;
            _gameConfig = gameConfig;
            _navigationPresenterService = navigationPresenterService;
        }

        public void OnCancel()
        {
            _accountService.Model.Account.ResetInsurance();
            _accountService.SaveAsync().Forget();
            PresenterService.Show<InsuranceCanceledView>().Forget();
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }
    }
}