using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLoanSplash
{
    public class BusinessLoanSplashViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLoanSplashViewController>().AsSingle().NonLazy();
        }
    }
}