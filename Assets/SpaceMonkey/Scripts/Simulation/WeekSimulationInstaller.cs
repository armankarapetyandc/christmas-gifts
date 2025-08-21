using Zenject;

namespace SpaceMonkey.Scripts.Simulation
{
    public class WeekSimulationInstaller : Installer<WeekSimulationInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WeekSimulationContext>().AsSingle().NonLazy();
            Container.BindFactory<WeekSimulation, WeekSimulation.Factory>();
        }
    }
}