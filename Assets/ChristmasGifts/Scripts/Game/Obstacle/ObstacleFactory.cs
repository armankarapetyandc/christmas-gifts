using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Config.Obstacles;
using ChristmasGifts.Scripts.Game.Common;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Obstacle
{
    public class ObstacleFactory : ObjectFactoryNavMesh
    {
        [SerializeField] private List<ObstacleConfig> obstacleConfigs;

        public Obstacle Create()
        {
            ObstacleConfig randomObstacleConfig = obstacleConfigs[Random.Range(0, obstacleConfigs.Count)];
            var instance = Create(randomObstacleConfig.Prefab, Quaternion.identity);
            if (instance == null)
            {
                return null;
            }

            Vector3 pos = instance.transform.position;
            pos.y = instance.transform.localScale.y / 2f;
            instance.transform.position = pos;
            instance.Setup(randomObstacleConfig);
            return instance;
        }
    }
}