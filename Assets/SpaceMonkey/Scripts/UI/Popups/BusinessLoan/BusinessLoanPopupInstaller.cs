using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLoan
{
    public class BusinessLoanPopupInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLoanPopupController>().AsSingle().NonLazy();
        }
    }
}