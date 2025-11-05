using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BusinessLocationSign
{
    public class BusinessLocationSignViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLocationSignViewController>().AsSingle().NonLazy();
        }
    }
}