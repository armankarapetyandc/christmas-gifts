using ChristmasGifts.Scripts.Game;
using ChristmasGifts.Scripts.Game.Obstacle;
using ChristmasGifts.Scripts.Game.Prize;
using UnityEngine;
using Zenject;

namespace ChristmasGifts.Scripts.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ObstacleGenerator obstacleGenerator;
        [SerializeField] private PrizeGenerator prizeGenerator;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Pointer pointer;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().WithId("MainCamera").FromInstance(mainCamera).AsSingle().NonLazy();
            Container.BindInterfacesTo<Pointer>().FromInstance(pointer).AsSingle().NonLazy();
            Container.BindInterfacesTo<PlayerController>().FromInstance(playerController).AsSingle().NonLazy();
            Container.BindInterfacesTo<PrizeGenerator>().FromInstance(prizeGenerator).AsSingle().NonLazy();
            Container.BindInterfacesTo<ObstacleGenerator>().FromInstance(obstacleGenerator).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
            GameLoader.Installer.Install(Container);
        }
    }
}