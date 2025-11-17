using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.Fire
{
    public class FirePopupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FirePopupController>().AsSingle().NonLazy();
        }
    }
}
