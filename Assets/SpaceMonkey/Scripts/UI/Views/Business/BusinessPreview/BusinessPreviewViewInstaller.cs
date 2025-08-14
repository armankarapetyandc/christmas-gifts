using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessPreview
{
    public class BusinessPreviewViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BusinessPreviewViewController>().AsSingle().NonLazy();
        }
    }
}