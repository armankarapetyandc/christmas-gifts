using Zenject;

namespace SpaceMonkey.Scripts.Simulation.Investment
{
    public class InvestmentInstaller : Installer<InvestmentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InvestmentSimulator>().AsSingle();
        }
    }
}