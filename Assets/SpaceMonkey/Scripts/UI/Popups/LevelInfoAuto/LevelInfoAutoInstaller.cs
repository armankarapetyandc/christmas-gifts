using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfoAuto
{
    public class LevelInfoAutoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelInfoAutoController>().AsSingle().NonLazy();
        }
    
    }
}