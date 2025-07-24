using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Startup
{
    public class StartupViewInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StartupViewController>().AsSingle().NonLazy();
        }
    }
}