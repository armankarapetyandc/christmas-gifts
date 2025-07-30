using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Map
{
    public class MapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MapController>().AsSingle().NonLazy();
        }
    }
}