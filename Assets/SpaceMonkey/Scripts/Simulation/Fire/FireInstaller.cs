using Zenject;

namespace SpaceMonkey.Scripts.Simulation.Fire
{
    public class FireInstaller : Installer<FireInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<FireSimulator>().AsSingle();
        }
    }
}
