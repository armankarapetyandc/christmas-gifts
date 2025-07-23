using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.CategorySelection
{
    public class CategorySelectionInstaller : MonoInstaller<CategorySelectionInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CategorySelectionController>().AsSingle().NonLazy();
        }
    }
}