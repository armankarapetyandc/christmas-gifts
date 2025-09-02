using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities
{
    public class OpportunitiesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<OpportunitiesController>().AsSingle().NonLazy();
        }
    }
}