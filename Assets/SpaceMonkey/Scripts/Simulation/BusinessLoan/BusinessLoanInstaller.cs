using Zenject;

namespace SpaceMonkey.Scripts.Simulation.BusinessLoan
{
    public class BusinessLoanInstaller : Installer<BusinessLoanInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLoanSimulator>()
                .AsSingle()
                .NonLazy();
        }
    }
}
