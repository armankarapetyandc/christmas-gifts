using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditcCard
{
    public class CreditCardViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CreditCardViewController>().AsSingle().NonLazy();
        }
    }
}