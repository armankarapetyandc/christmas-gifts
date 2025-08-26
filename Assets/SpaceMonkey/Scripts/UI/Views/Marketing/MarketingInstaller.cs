using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Marketing
{
    public class MarketingInstaller : MonoInstaller
    { 
        public override void InstallBindings()
        {
            Container.Bind<MarketingController>().AsSingle().NonLazy();
        }
    }
}