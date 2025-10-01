using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.LevelInfo
{
    public class LevelInfoInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelInfoController>().AsSingle().NonLazy();
        }
    }
}