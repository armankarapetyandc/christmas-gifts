using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.ProductionAlert
{
    public class ProductionAlertInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProductionAlertController>().AsSingle().NonLazy();
        }
    }
}