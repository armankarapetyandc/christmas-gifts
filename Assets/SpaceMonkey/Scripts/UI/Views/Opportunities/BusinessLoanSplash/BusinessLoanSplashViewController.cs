using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement;
using UIService.Runtime.Presenter;
using UIService.Runtime.Presenter.Base;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanSplash
{
    public class BusinessLoanSplashViewController : BasePresenterController
    {
        public BusinessLoanSplashViewController(PresenterService presenterService) : base(presenterService)
        {
        }
        
        internal void OnNext()
        {
            PresenterService.Show<BusinessLoanStatementView>(new BusinessLoanStatementView.Data()).Forget();
        }
    }
}