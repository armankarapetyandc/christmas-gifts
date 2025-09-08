using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff.HiredStaff
{
    public class HiredStaffInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<HiredStaffController>().AsSingle().NonLazy();
        }
    }
}