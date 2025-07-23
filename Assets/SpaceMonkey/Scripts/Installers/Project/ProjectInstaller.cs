using ContextLoaderService.Runtime;
using SpaceMonkey.Scripts.UI.Asset.BusinessIdeas;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Popups.Core;
using UIService.Runtime.Core;
using UIService.Runtime.Installers;
using UIService.Runtime.Presenter;
using UnityEngine;
using Zenject;

namespace SpaceMonkey.Scripts.Installers.Project
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        [SerializeField] private LoadingView loadingViewPrefab;
        [SerializeField] private PresenterView presenterViewPrefab;
        [SerializeField] private PopupPresenterView popupPresenterViewPrefab;
        [SerializeField] private NavigationPresenterView navigationPresenterViewPrefab;
        [SerializeField] private BusinessIdeaAssetDatabase businessIdeaAssetDatabase;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<LoadingService>()
                .AsSingle()
                .NonLazy();

            UIServiceInstaller.Install(Container);
            Container.Bind<PopupPresenterService>().AsSingle().NonLazy();
            Container.Bind<NavigationPresenterService>().AsSingle().NonLazy();

            Container
                .BindInterfacesAndSelfTo<LoadingView>()
                .FromInstance(Instantiate(loadingViewPrefab))
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PresenterView>()
                .FromMethod(GetPresenterInstance)
                .AsSingle()
                .NonLazy();
            Container
                .BindInterfacesAndSelfTo<PopupPresenterView>()
                .FromMethod(GetPopupPresenterInstance)
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<NavigationPresenterView>()
                .FromMethod(GetNavigationPresenterInstance)
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<BusinessIdeaAssetDatabase>()
                .FromInstance(businessIdeaAssetDatabase)
                .AsSingle();
        }

        private PresenterView GetPresenterInstance(InjectContext ctx)
        {
            return Instantiate(presenterViewPrefab);
        }

        private PopupPresenterView GetPopupPresenterInstance(InjectContext ctx)
        {
            return Instantiate(popupPresenterViewPrefab);
        }

        private NavigationPresenterView GetNavigationPresenterInstance(InjectContext ctx)
        {
            return Instantiate(navigationPresenterViewPrefab);
        }
    }
}