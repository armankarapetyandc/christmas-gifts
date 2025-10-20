using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderDetails
{
    public class BigOrderDetailsViewInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BigOrderDetailsViewController>().AsSingle().NonLazy();
        }
    }
}