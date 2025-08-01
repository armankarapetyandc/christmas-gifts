using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product
{
    public class ProductCreationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProductCreationController>().AsSingle().NonLazy();
        }
    }
}