using Zenject;

namespace SpaceMonkey.Scripts.UI.Navigation.Bottom
{
    public class MainNavigationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainNavigationController>().AsSingle().NonLazy();
        }
    }
}