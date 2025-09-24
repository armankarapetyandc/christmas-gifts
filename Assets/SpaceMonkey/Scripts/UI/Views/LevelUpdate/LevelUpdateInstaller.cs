using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.LevelUpdate
{
    public class LevelUpdateInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelUpdateController>().ToSelf().AsSingle();
        }
    }
}