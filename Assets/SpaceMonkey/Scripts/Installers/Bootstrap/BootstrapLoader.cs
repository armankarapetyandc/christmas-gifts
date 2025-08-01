using System;
using ContextLoaderService.Runtime;
using ContextLoaderService.Runtime.BaseUnits;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Core.ContextLoader;
using SpaceMonkey.Scripts.UI.Views.Splash;
using UIService.Runtime.Presenter;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using IInitializable = Zenject.IInitializable;
using Logger = DCLogger.Runtime.Logger;

namespace SpaceMonkey.Scripts.Installers.Bootstrap
{
    public class BootstrapLoader : IInitializable
    {
        private readonly BootstrapParams _params;
        private readonly LoadingService _loadingService;
        private readonly PresenterService _presenterService;

        public class Installer : Installer<BootstrapParams, Installer>
        {
            private readonly BootstrapParams _params;

            public Installer(BootstrapParams @params)
            {
                _params = @params;
            }

            public override void InstallBindings()
            {
                Container.BindInterfacesTo<BootstrapLoader>().AsSingle().WithArguments(_params).NonLazy();
            }
        }

        public BootstrapLoader(BootstrapParams @params, LoadingService loadingService, PresenterService presenterService)
        {
            _params = @params;
            _loadingService = loadingService;
            _presenterService = presenterService;
        }

        void IInitializable.Initialize()
        {
            LoadAsync().Forget();
        }

        private async UniTaskVoid LoadAsync()
        {
            try
            {
                Logger.Log($"Path: {Application.persistentDataPath}", SpaceMonkeyLogChannels.Default);
                var configUnit = new ResourceLoadingUnit<BootstrapConfig>(_params.BoostrapConfigPath);
                await _loadingService.BeginLoading(configUnit);
                Application.targetFrameRate = configUnit.Result.TargetFrameRate;
                var sceneUnit = new SceneLoadUnit<string>(configUnit.Result.MainSceneName, LoadSceneMode.Single);
                await _loadingService.BeginLoading(sceneUnit);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, SpaceMonkeyLogChannels.Default);
            }
        }
    }
}