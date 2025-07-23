using SpaceMonkey.Scripts.Profile;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Main
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AccountService>().AsSingle();
            MainLoader.Installer.Install(Container);
        }
    }
}