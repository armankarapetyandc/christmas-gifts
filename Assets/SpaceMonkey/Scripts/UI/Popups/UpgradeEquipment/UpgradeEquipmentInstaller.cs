using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.UpgradeEquipment
{
    public class UpgradeEquipmentInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UpgradeEquipmentController>().AsSingle().NonLazy();
        }
    }
}