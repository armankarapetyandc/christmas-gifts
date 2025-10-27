using SpaceMonkey.Scripts.Cloud;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.Utilities;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Main
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        [SerializeField] private CameraHolder cameraHolder;
        public override void InstallBindings()
        {
            Container.Bind<CameraHolder>().FromInstance(cameraHolder).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AccountService>().AsSingle();
            CloudDataInstaller.Install(Container);
            WeekSimulationInstaller.Install(Container);
            MainLoader.Installer.Install(Container);
            CreditCardInstaller.Install(Container);
            BusinessLoanInstaller.Install(Container);
        }
    }
}