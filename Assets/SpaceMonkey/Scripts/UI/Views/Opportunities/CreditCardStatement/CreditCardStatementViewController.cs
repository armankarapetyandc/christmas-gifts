using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement
{
    public class CreditCardStatementViewController : BasePresenterController
    {
        private readonly NavigationPresenterService _navigationPresenterService;
        private readonly CreditSimulator _creditSimulator;

        public CreditCardStatementViewController(PresenterService presenterService,
            NavigationPresenterService navigationPresenterService, CreditSimulator creditSimulator) : base(presenterService)
        {
            _navigationPresenterService = navigationPresenterService;
            _creditSimulator = creditSimulator;
        }

        public GameData gameData => _creditSimulator.Data;
        public double minimumPayment => _creditSimulator.GetMinimumPayment();
        public List<Transaction> Transactions => _creditSimulator.Transactions;
        
        public void SelectPayment(PaymentOption option)
        {
            _creditSimulator.SelectPayment(option);
        }
        
        public void CloseView()
        {
            PresenterService.Hide();
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