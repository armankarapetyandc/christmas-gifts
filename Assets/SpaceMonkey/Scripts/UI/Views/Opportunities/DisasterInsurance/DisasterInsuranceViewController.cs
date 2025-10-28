using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Opportunities;
using SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsuranceCongratulation;
using SpaceMonkey.Scripts.Utilities;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.DisasterInsurance
{
    public class DisasterInsuranceViewController : BasePresenterController
    {
        private NavigationPresenterService _navigationPresenterService;
        private AccountService _accountService;
        private GameConfig _gameConfig;

        public float Money => _accountService.Model.Account.Money;
        public int InsurancePrice => _gameConfig.InsuranceInfo.InsurancePrice;

        public DisasterInsuranceViewController(NavigationPresenterService navigationPresenterService,
            AccountService accountService, GameConfig gameConfig,
            PresenterService presenterService) : base(presenterService)
        {
            _gameConfig = gameConfig;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
        }

        public async void PurchasePolicy()
        {
            _accountService.Model.Account.CreateInsuranceData(_gameConfig.InsuranceInfo);
            await _accountService.SaveAsync();
            PresenterService.Show<DisasterInsuranceCongratulationView>().Forget();
            
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