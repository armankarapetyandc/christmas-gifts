using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.DisasterInsurance
{
    public class DisasterInsuranceViewInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DisasterInsuranceViewController>().AsSingle().NonLazy();
        }
    }
}