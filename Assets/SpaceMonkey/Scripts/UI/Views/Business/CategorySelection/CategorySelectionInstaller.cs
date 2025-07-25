using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class CategorySelectionInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CategorySelectionController>().AsSingle().NonLazy();
        }
    }
}