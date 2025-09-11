using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.DeleteProduct
{
    public class DeleteProductPopupInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DeleteProductPopupController>().AsSingle().NonLazy();
        }
    }
}