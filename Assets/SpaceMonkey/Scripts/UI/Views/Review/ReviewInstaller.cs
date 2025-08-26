using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Review
{
    public class ReviewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ReviewController>().AsSingle().NonLazy();
        }
    }
}