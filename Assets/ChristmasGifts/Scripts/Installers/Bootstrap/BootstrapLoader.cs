using System;
using System.Threading;
using ChristmasGifts.Scripts.Common.Unit.BaseUnits;
using ChristmasGifts.Scripts.Config;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ChristmasGifts.Scripts.Installers.Bootstrap
{
    public class BootstrapLoader : IInitializable
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<BootstrapLoader>().AsSingle().NonLazy();
            }
        }

        private const string BootstrapConfigPath = "Configs/BootstrapConfig";

        void IInitializable.Initialize()
        {
            Load().Forget();
        }

        private async UniTaskVoid Load(CancellationToken cancellationToken = default)
        {
            try
            {
                var bootstrapConfigProcessingUnit = new ResourceProcessingUnit<BootstrapConfig>(BootstrapConfigPath);
                await bootstrapConfigProcessingUnit.Execute(cancellationToken);
                Application.targetFrameRate = bootstrapConfigProcessingUnit.Result.TargetFrameRate;
                var mainSceneProcessingUnit =
                    new SceneProcessingUnit(bootstrapConfigProcessingUnit.Result.MainSceneName, LoadSceneMode.Single);
                await mainSceneProcessingUnit.Execute(cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(BootstrapLoader)}] Exception: {e}");
            }
        }
    }
}