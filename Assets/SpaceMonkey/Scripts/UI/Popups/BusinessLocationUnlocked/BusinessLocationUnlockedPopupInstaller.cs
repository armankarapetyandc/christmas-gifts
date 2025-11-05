using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.BusinessLocationUnlocked
{
    public class BusinessLocationUnlockedPopupInstaller : MonoInstaller
    { 
        public override void InstallBindings()
        {
            Container.Bind<BusinessLocationUnlockedPopupController>().AsSingle().NonLazy();
        }
    }
}