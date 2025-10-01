using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.BusinessExamples
{
    public class BusinessExamplesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessExamplesController>().AsSingle().NonLazy();
        }
    }
}