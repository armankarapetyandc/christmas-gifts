using Zenject;

namespace SpaceMonkey.Scripts.UI.Popups.Competition
{
    public class CompetitionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CompetitionController>().AsSingle().NonLazy();
        }
    }
}