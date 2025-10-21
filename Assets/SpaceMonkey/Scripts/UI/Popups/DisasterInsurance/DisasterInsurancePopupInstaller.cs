using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.DisasterInsurance
{
    public class DisasterInsurancePopupInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DisasterInsurancePopupController>().AsSingle().NonLazy();
        }
    }
}