using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLoanInfo
{
    public class BusinessLoanInfoPopupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessLoanInfoPopupController>().AsSingle().NonLazy();
        }
    }
}