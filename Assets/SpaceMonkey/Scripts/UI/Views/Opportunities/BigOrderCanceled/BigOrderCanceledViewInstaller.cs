using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCanceled
{
    public class BigOrderCanceledViewInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BigOrderCanceledViewController>().AsSingle().NonLazy();
        }
    }
}