using AudioPlayer;
using AudioPlayerService.Runtime;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using SpaceMonkey.Scripts.UI.Popups.CreditCardInfo;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardDecline;
using SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard
{
    public class CreditCardViewController: BasePresenterController
    {
        private readonly PopupPresenterService _popupPresenterService;
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly AccountService _accountService;
        private readonly CreditSimulator _creditSimulator;

        public CreditCardViewController(PresenterService presenterService, PopupPresenterService popupPresenterService,
            NavigationPresenterService navigationPresenterService, AccountService accountService, 
            CreditSimulator creditSimulator) : base(presenterService)
        {
            _popupPresenterService = popupPresenterService;
            _navigationPresenterService = navigationPresenterService;
            _accountService = accountService;
            _creditSimulator = creditSimulator;
        }

        internal void OnBack()
        {
            _navigationPresenterService.Show<MainNavigation>(new MainNavigation.Data
            {
                Type = MainNavigationType.Opportunities
            }).Forget();
        }

        internal void OnNext()
        {
            if (_accountService.Model.Account.Week >= 3)
            {
                SfxPlayer.Play(Sounds.Click_Next);
                _creditSimulator.ApplyForCredit().Forget();
                PresenterService.Show<CreditCardSplashView>().Forget();   
            }
            else
            {
                PresenterService.Show<CreditCardDeclineView>().Forget();   
            }
        }

        public void OnInfo()
        {
            _popupPresenterService.Show<CreditCardInfoPopup>().Forget();
        }
    }
}