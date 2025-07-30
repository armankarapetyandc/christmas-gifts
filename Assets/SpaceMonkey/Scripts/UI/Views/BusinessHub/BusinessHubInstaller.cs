using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class BusinessHubInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessHubController>().AsSingle().NonLazy();
        }
    }
}