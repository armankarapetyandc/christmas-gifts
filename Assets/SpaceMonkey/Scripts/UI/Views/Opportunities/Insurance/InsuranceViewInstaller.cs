using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.Insurance
{
    public class InsuranceViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InsuranceViewController>().AsSingle().NonLazy();
        }
    }
}