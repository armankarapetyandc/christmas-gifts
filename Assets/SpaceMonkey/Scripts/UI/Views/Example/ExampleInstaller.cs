using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Example
{
    public class ExampleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ExampleController>().AsSingle().NonLazy();
        }
    }
}