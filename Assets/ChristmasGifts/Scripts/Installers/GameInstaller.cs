using ChristmasGifts.Scripts.Game;
using Zenject;

namespace ChristmasGifts.Scripts.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
            GameLoader.Installer.Install(Container);
        }
    }
}