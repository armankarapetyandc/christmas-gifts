using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessIconBuilder
{
    public class BusinessIconBuilderViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessIconBuilderViewController>().AsSingle().NonLazy();
        }
    }
}