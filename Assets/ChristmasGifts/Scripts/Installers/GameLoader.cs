using ChristmasGifts.Scripts.Game;
using Zenject;

namespace ChristmasGifts.Scripts.Installers
{
    public class GameLoader : IInitializable
    {
        private readonly GameController _gameController;

        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<GameLoader>().AsSingle().NonLazy();
            }
        }


        public GameLoader(GameController gameController)
        {
            _gameController = gameController;
        }

        void IInitializable.Initialize()
        {
            _gameController.Run();
        }
    }
}