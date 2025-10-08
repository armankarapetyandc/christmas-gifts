using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.UpgradeCapacity
{
    public class WeekEndRewardInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WeekEndRewardController>().AsSingle().NonLazy();
        }
    }
}