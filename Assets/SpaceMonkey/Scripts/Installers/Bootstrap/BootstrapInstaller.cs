using System.Collections.Generic;
using SpaceMonkey.Scripts.Analytics;
using SpaceMonkey.Scripts.Analytics.Service;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] private BootstrapParams @params;
        [SerializeField] private EventSystem eventSystem;

        public override void InstallBindings()
        {
            DontDestroyOnLoad(eventSystem);
            AnalyticsInstaller.Install(Container, new List<IAnalyticsProvider>
            {
                new GoogleAnalyticsProvider()
            });
            BootstrapLoader.Installer.Install(Container, @params);
        }
    }
}