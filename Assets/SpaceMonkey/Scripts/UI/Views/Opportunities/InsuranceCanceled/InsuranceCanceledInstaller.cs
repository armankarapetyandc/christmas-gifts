using Zenject;

namespace SpaceMonkey.Scripts.UI.Views.Opportunities.InsuranceCanceled
{
    public class InsuranceCanceledInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InsuranceCanceledViewController>().AsSingle().NonLazy();
        }

    } 
}