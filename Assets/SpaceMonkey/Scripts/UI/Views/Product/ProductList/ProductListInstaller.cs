using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Product.ProductList
{
    public class ProductListInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProductListController>().AsSingle().NonLazy();
        }
    }
}