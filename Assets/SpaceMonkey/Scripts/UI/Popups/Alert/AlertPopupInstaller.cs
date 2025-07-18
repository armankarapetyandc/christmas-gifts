using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.Alert
{
    public class AlertPopupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AlertPopupController>().AsSingle().NonLazy();
        }
    }
}