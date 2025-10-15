using System;
using System.Threading.Tasks;
using ContextLoaderService.Runtime;
using Cysharp.Threading.Tasks;
using DCLogger.Runtime;
using SpaceMonkey.Scripts.Cloud;
using SpaceMonkey.Scripts.Cloud.Config.GameConfig;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using SpaceMonkey.Scripts.UI;
using SpaceMonkey.Scripts.UI.Navigation.Bottom;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.Splash;
using SpaceMonkey.Scripts.UI.Views.Startup;
using UIService.Runtime.Presenter;
using UnityEngine.Rendering;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Main
{
    public class MainLoader : BaseLoader
    {
        private readonly PresenterService _presenterService;
        private readonly AccountService _accountService;
        private readonly CloudDataService _cloudDataService;
        private readonly GameConfig _gameConfig;
        private readonly CustomerReviewConfig _customerReviewConfig;
        private readonly NavigationPresenterService _navigationPresenterService;

        public MainLoader(LoadingService loadingService, PresenterService presenterService,
            AccountService accountService, CloudDataService cloudDataService, GameConfig gameConfig,CustomerReviewConfig customerReviewConfig,
            NavigationPresenterService navigationPresenterService) : base(loadingService)
        {
            _presenterService = presenterService;
            _accountService = accountService;
            _cloudDataService = cloudDataService;
            _gameConfig = gameConfig;
            _customerReviewConfig = customerReviewConfig;
            _navigationPresenterService = navigationPresenterService;
        }

        protected override async UniTask Load()
        {
            try
            {
                await LoadingService.BeginLoading(UniTask.DelayFrame(1).ToLoadingUnit());

                var cloudDataClientUnit = _cloudDataService.Initialize();
                await LoadingService.BeginLoading(cloudDataClientUnit);

                await PatchGameConfig();

                if (_accountService.IsFreshAccount)
                {
                    await LoadingService.BeginLoading(_presenterService.Show<SplashView>().ToLoadingUnit());
                    await UniTask.Delay(2000);
                    await LoadingService.BeginLoading(_presenterService.Show<StartupView>().ToLoadingUnit());
                    return;
                }

                await LoadingService.BeginLoading(
                    _accountService.LoadAsync().ToLoadingUnit(),
                    _navigationPresenterService.Show<MainNavigation>().ToLoadingUnit()
                );
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, SpaceMonkeyLogChannels.Default);
            }
        }

        private async UniTask PatchGameConfig()
        {
#if UNITY_EDITOR
            if (!Cloud.Tool.CloudMenuTools.GetState())
            {
                return;
            }
#endif

            var categoriesUnit = _cloudDataService.Patch<CategoryInfoPatcher, CategoryInfo[]>();
            var productionLevelsUnit = _cloudDataService.Patch<ProductionLevelInfoPatcher, ProductionLevelInfo[]>();
            var marketingInfosUnit = _cloudDataService.Patch<MarketingInfoPatcher, MarketingInfo[]>();
            var businessExamplesUnit = _cloudDataService.Patch<BusinessExamplePatcher, BusinessExample[]>();
            var staffsUnit = _cloudDataService.Patch<StaffPatcher, Staff[]>();
            var creditCardInfoUnit = _cloudDataService.Patch<CreditCardInfoPatcher, CreditCardInfo>();
            var simulationInfoUnit = _cloudDataService.Patch<SimulationInfoPatcher, SimulationInfo>();
            var customerReviewUnit = _cloudDataService.Patch<CustomerReviewPatcher, ReviewInfo[]>();

            await LoadingService.BeginLoadingParallel(
                categoriesUnit, productionLevelsUnit, marketingInfosUnit,
                businessExamplesUnit, staffsUnit, creditCardInfoUnit,
                simulationInfoUnit, customerReviewUnit
            );

            _gameConfig.PatchCategories(categoriesUnit.Result);
            _gameConfig.PatchProductionLevelInfos(productionLevelsUnit.Result);
            _gameConfig.PatchMarketingInfos(marketingInfosUnit.Result);
            _gameConfig.PatchBusinessExamples(businessExamplesUnit.Result);
            _gameConfig.PatchStaffs(staffsUnit.Result);
            _gameConfig.PatchCreditCardInfo(creditCardInfoUnit.Result);
            _gameConfig.PatchSimulationInfo(simulationInfoUnit.Result);

            _customerReviewConfig.PatchReviewInfo(customerReviewUnit.Result);
        }

        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<MainLoader>().AsSingle().NonLazy();
            }
        }
    }
}