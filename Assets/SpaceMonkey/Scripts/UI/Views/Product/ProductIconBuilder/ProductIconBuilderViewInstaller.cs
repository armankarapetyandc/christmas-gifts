using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductIconBuilder
{
    public class ProductIconBuilderViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProductIconBuilderController>().AsSingle().NonLazy();
        }
    }
}