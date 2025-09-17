using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.MutualFunds
{
    public class MutualFundsViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MutualFundsViewController>().AsSingle().NonLazy();
        }
    }
}