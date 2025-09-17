using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.Investing
{
    public class InvestingViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InvestingViewController>().AsSingle().NonLazy();
        }
    }
}