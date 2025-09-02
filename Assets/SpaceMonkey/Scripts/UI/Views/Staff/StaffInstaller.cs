using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Staff
{
    public class StaffInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StaffController>().AsSingle().NonLazy();
        }
    }
}