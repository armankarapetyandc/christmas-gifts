using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCardInfo
{
    public class CreditCardInfoPopupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CreditCardInfoPopupController>().AsSingle().NonLazy();
        }
    }
}