using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanSplash
{
    public class BusinessLoanSplashViewController : BasePresenterController
    {
        private readonly BusinessLoanSimulator _businessLoanSimulator;

        public BusinessLoanSplashViewController(PresenterService presenterService, 
            BusinessLoanSimulator businessLoanSimulator) : base(presenterService)
        {
            _businessLoanSimulator = businessLoanSimulator;
        }
        
        internal async UniTask OnNext()
        {
            await _businessLoanSimulator.ApplyForLoan();
            PresenterService.Show<BusinessLoanStatementView>(new BusinessLoanStatementView.Data()).Forget();
        }
    }
}