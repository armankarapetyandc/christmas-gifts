using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Orders
{
    public class OrdersViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<OrdersViewController>().AsSingle().NonLazy();
        }
    }
}