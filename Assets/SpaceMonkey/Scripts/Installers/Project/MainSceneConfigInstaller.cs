using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset.IconBuilder;
using SpaceMonkey.Scripts.UI.Asset.Product;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Project
{
    [CreateAssetMenu(fileName = "Main scene config installer", menuName = "Space Monkey/Installers/Main Scene Config")]

    public class MainSceneConfigInstaller : ScriptableObjectInstaller<MainSceneConfigInstaller>
    {
        [SerializeField] private IconBuilderConfig iconBuilderConfig;
        [FormerlySerializedAs("productBuilderConfig")] [SerializeField] private ProductIconBuilderConfig productIconBuilderConfig;
        [SerializeField] private HashTagsConfig hashTagsConfig;

        public override void InstallBindings()
        {
            Container.Bind<IconBuilderConfig>().FromInstance(iconBuilderConfig).AsSingle().NonLazy();
            Container.Bind<HashTagsConfig>().FromInstance(hashTagsConfig).AsSingle().NonLazy();
            Container.Bind<ProductIconBuilderConfig>().FromInstance(productIconBuilderConfig).AsSingle().NonLazy();
        }
        
    }
}