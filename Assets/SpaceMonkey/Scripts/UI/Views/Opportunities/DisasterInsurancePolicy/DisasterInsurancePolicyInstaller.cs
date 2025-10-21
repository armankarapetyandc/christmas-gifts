using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.DisasterInsurancePolicy
{
    public class DisasterInsurancePolicyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DisasterInsurancePolicyViewController>().AsSingle().NonLazy();
        }

    } 
}