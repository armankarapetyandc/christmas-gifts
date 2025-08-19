using SpaceMonkey.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Project
{
    [CreateAssetMenu(fileName = "Main scene config installer", menuName = "Space Monkey/Installers/Main Scene Config")]
    public class MainSceneConfigInstaller : ScriptableObjectInstaller<MainSceneConfigInstaller>
    {
        [SerializeField] private GameConfig gameConfig;

        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
        }
    }
}