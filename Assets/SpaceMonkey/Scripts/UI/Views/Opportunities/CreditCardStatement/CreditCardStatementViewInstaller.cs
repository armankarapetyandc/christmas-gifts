using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.CreditCardStatement
{
    public class CreditCardStatementViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CreditCardStatementViewController>().AsSingle().NonLazy();
        }
    }
}