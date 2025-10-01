using Zenject;

namespace SpaceMonkey.Scripts.Simulation.CreditCard
{
    public class CreditCardInstaller: Installer<CreditCardInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CreditSimulator>().AsSingle();
        }
    }
}