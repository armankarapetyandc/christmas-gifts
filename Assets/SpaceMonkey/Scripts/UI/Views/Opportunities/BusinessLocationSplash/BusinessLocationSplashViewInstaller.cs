using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSplash
{
    public class BusinessLocationSplashViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLocationSplashViewController>().AsSingle().NonLazy();
        }
    }
}