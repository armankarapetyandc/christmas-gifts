using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder
{
    public class BigOrderViewInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BigOrderViewController>().AsSingle().NonLazy();
        }
    }
}