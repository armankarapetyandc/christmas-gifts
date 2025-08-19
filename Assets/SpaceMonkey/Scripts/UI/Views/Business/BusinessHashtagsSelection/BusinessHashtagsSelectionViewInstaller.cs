using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessHashtagsSelection
{
    public class BusinessHashtagsSelectionViewInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessHashtagsSelectionViewController>().AsSingle().NonLazy();
        }
    }
}