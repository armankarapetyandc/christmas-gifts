using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.ProfitAndLoss
{
    public class ProfitInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProfitController>().AsSingle().NonLazy();
        }
    }
}