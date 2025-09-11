using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardSplash
{
    public class CreditCardSplashViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CreditCardSplashViewController>().AsSingle().NonLazy();
        }
    }
}