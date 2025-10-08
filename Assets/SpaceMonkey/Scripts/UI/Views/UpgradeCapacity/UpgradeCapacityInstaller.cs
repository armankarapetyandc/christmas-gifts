using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.UpgradeCapacity
{
    public class UpgradeCapacityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UpgradeCapacityController>().AsSingle().NonLazy();
        }
    }
}