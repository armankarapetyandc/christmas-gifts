using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanStatement
{
    public class BusinessLoanStatementViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLoanStatementViewController>().AsSingle().NonLazy();
        }
    }
}