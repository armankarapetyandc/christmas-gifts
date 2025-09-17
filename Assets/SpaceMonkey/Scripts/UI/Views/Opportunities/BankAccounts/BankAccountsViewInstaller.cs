using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BankAccounts
{
    public class BankAccountsViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BankAccountsViewController>().AsSingle().NonLazy();
        }
    }
}