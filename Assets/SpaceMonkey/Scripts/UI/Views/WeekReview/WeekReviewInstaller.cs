using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.WeekReview
{
    public class WeekReviewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WeekReviewController>().AsSingle().NonLazy();
        }
    }
}