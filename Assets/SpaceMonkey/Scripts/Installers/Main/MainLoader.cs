using System;
using ContextLoaderService.Runtime;
using Cysharp.Threading.Tasks;
using DCLogger.Runtime;
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
        private readonly NavigationPresenterService _navigationPresenterService;

        public MainLoader(LoadingService loadingService, PresenterService presenterService,
            AccountService accountService,
            NavigationPresenterService navigationPresenterService) : base(loadingService)
        {
            _presenterService = presenterService;
            _accountService = accountService;
            _navigationPresenterService = navigationPresenterService;
        }

        protected override async UniTask Load()
        {
            try
            {
                await LoadingService.BeginLoading(UniTask.DelayFrame(1).ToLoadingUnit());

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

        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<MainLoader>().AsSingle().NonLazy();
            }
        }
    }
}