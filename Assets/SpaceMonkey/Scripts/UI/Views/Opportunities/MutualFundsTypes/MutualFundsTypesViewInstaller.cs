using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.MutualFundsTypes
{
    public class MutualFundsTypesViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MutualFundsTypesViewController>().AsSingle().NonLazy();
        }
    }
}