using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoan
{
    public class BusinessLoanViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLoanViewController>().AsSingle().NonLazy();
        }
    }
}