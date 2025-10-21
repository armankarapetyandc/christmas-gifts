using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsuranceCongratulation
{
    public class DisasterInsuranceCongratulationInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DisasterInsuranceCongratulationViewController>().AsSingle().NonLazy();
        }
    }
}