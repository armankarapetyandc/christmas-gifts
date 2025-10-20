using System.Collections.Generic;
using Zenject;

namespace SpaceMonkey.Scripts.Analytics.Service
{
    public class AnalyticsInstaller : Installer<List<IAnalyticsProvider>, AnalyticsInstaller>
    {
        private readonly List<IAnalyticsProvider> _providers;

        public AnalyticsInstaller(List<IAnalyticsProvider> providers)
        {
            _providers = providers;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AnalyticsService>().AsSingle().Lazy();
            Container.Bind<List<IAnalyticsProvider>>().FromInstance(_providers).AsSingle().Lazy();
        }
    }
}