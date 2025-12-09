using System.Collections.Generic;
using SpaceMonkey.Scripts.Analytics;
using SpaceMonkey.Scripts.Analytics.Service;
using SpaceMonkey.Scripts.Cloud;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.Simulation;
using SpaceMonkey.Scripts.Simulation.BusinessLoan;
using SpaceMonkey.Scripts.Simulation.CreditCard;
using SpaceMonkey.Scripts.Simulation.Fire;
using SpaceMonkey.Scripts.Simulation.Investment;
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
            AnalyticsInstaller.Install(Container, new List<IAnalyticsProvider>
            {
                new GoogleAnalyticsProvider()
            });
            Container.BindInterfacesAndSelfTo<AnalyticsProvider>().AsSingle().Lazy();
            
            Container.Bind<CameraHolder>().FromInstance(cameraHolder).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AccountService>().AsSingle();
            CloudDataInstaller.Install(Container);
            WeekSimulationInstaller.Install(Container);
            MainLoader.Installer.Install(Container);
            CreditCardInstaller.Install(Container);
            BusinessLoanInstaller.Install(Container);
            InvestmentInstaller.Install(Container);
            FireInstaller.Install(Container);
        }
    }
}