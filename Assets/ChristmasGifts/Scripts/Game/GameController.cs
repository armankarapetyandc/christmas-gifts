using UnityEngine;

namespace ChristmasGifts.Scripts.Game
{
    public class GameController
    {
        private readonly IGameRun[] _gameRuns;

        public GameController(IGameRun[] gameRuns)
        {
            _gameRuns = gameRuns;
        }

        public void Run()
        {
            foreach (IGameRun gameRun in _gameRuns)
            {
                gameRun.Run();
            }

            Debug.Log($"GameController: RUN!");
        }
    }
}