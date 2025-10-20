using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.BigOrder
{
    public class BigOrderPopupInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BigOrderPopupController>().AsSingle().NonLazy();
        }
    }
}