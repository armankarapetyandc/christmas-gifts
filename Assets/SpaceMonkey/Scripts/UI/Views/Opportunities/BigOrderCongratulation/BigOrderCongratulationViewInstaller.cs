using SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrder;
using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.BigOrderCongratulation
{
    public class BigOrderCongratulationViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BigOrderCongratulationViewController>().AsSingle().NonLazy();
        }
    }
}