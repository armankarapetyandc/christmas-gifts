using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Splash
{
    public class SplashViewInstaller : MonoInstaller<SplashViewInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<SplashViewController>().AsSingle().NonLazy();
        }
    }
}