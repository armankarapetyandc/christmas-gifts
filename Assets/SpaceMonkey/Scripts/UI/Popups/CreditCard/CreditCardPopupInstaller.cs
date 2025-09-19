using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.CreditCard
{
    public class CreditCardPopupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CreditCardPopupController>().AsSingle().NonLazy();
        }
    }
}