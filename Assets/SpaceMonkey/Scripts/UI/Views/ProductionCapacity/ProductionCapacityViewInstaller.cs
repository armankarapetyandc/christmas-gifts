using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProductionCapacity
{
    public class ProductionCapacityViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProductionCapacityViewController>().ToSelf().AsSingle();
        }
    }
}