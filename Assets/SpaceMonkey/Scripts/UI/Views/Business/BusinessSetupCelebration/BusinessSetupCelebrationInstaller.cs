using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetupCelebration
{
    public class BusinessSetupCelebrationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessSetupCelebrationController>().AsSingle().NonLazy();
        }
    }
}