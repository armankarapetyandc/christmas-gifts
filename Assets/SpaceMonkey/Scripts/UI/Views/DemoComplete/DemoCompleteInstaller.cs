using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.DemoComplete
{
    public class DemoCompleteInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DemoCompleteController>().AsSingle().NonLazy();
        }
    }
}