using System;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Configs.Map;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Project
{
    [CreateAssetMenu(fileName = "Main scene config installer", menuName = "Space Monkey/Installers/Main Scene Config")]
    public class MainSceneConfigInstaller : ScriptableObjectInstaller<MainSceneConfigInstaller>
    {
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private MapConfig mapConfig;
        [SerializeField] private CustomerReviewConfig customerReviewConfig;
        [SerializeField] private ScoresConfigs scoresConfigs;

        public override void InstallBindings()
        {
#if UNITY_EDITOR
            if (Cloud.Tool.CloudMenuTools.GetState())
            {
                BindScriptableObjectFromNew(gameConfig).AsSingle().NonLazy();
                BindScriptableObjectFromNew(customerReviewConfig).AsSingle().NonLazy();
            }
            else
            {
                Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
                Container.Bind<CustomerReviewConfig>().FromInstance(customerReviewConfig).AsSingle().NonLazy();
            }
#else
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
            Container.Bind<CustomerReviewConfig>().FromInstance(customerReviewConfig).AsSingle().NonLazy();
#endif
            Container.Bind<MapConfig>().FromInstance(mapConfig).AsSingle().NonLazy();
            Container.Bind<ScoresConfigs>().FromInstance(scoresConfigs).AsSingle().NonLazy();
        }

        private ScopeConcreteIdArgConditionCopyNonLazyBinder BindScriptableObjectFromNew<T>(T original,
            Action<T> onCreated = null) where T : ScriptableObject
        {
            var instance = ScriptableObject.Instantiate(original);
            onCreated?.Invoke(instance);
            return Container.Bind<T>().FromInstance(instance);
        }
    }
}