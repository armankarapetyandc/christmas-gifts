using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.FireRepairSplash
{
    public class FireRepairSplashViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FireRepairSplashViewController>().AsSingle().NonLazy();
        }
    }
}
