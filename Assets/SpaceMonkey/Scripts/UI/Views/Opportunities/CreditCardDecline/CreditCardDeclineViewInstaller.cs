using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardDecline
{
    public class CreditCardDeclineViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CreditCardDeclineViewController>().AsSingle().NonLazy();
        }
    }
}