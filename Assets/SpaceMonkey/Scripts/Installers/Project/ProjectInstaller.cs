using ContextLoaderService.Runtime;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Project
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        [SerializeField] private LoadingView loadingViewPrefab;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<LoadingService>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<LoadingView>()
                .FromInstance(Instantiate(loadingViewPrefab))
                .AsSingle();
        }
    }
}