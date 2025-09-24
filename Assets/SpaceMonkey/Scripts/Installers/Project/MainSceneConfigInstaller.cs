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
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
            Container.Bind<MapConfig>().FromInstance(mapConfig).AsSingle().NonLazy();
            Container.Bind<CustomerReviewConfig>().FromInstance(customerReviewConfig).AsSingle().NonLazy();
            Container.Bind<ScoresConfigs>().FromInstance(scoresConfigs).AsSingle().NonLazy();
        }
    }
}