using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup
{
    public class BusinessSetupInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessSetupController>().AsSingle().NonLazy();
        }
    }
}