using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.NewProduct
{
    public class ProductInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProductController>().AsSingle().NonLazy();
        }
    }
}