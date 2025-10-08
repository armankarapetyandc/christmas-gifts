using Zenject;

namespace SpaceMonkey.Scripts.Cloud
{
    public class CloudDataInstaller : Installer<CloudDataInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CloudDataRestClient>().AsSingle().NonLazy();
            Container.Bind<CloudDataService>().AsSingle().NonLazy();
        }
    }
}