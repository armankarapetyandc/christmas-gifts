using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff.StaffManagement
{
    public class StaffManagementInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StaffManagementController>().AsSingle().NonLazy();
        }
    }
}