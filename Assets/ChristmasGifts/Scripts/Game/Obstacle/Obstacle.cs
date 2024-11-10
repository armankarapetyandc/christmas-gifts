using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Config.Obstacles;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        private ObstacleConfig _config;

        public void Setup(ObstacleConfig config)
        {
            _config = config;
        }
    }
}