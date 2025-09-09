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

        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
            Container.Bind<MapConfig>().FromInstance(mapConfig).AsSingle().NonLazy();
        }
    }
}