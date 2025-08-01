using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Project
{
    [CreateAssetMenu(fileName = "Main scene config installer", menuName = "Space Monkey/Installers/Main Scene Config")]
    public class MainSceneConfigInstaller : ScriptableObjectInstaller<MainSceneConfigInstaller>
    {
        [SerializeField] private IconBuilderConfig iconBuilderConfig;
        [SerializeField] private GameConfig gameConfig;

        public override void InstallBindings()
        {
            Container.Bind<IconBuilderConfig>().FromInstance(iconBuilderConfig).AsSingle().NonLazy();
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
        }
    }
}