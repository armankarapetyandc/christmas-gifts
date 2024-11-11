using System;
using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Config.Obstacles;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Obstacle
{
    public class ObstacleGenerator : MonoBehaviour, IGameRun
    {
        [SerializeField] private ObstacleGeneratorConfig generatorConfig;
        [SerializeField] private ObstacleFactory factory;

        private readonly List<Obstacle> _obstacles = new List<Obstacle>();

        public void Run()
        {
            PopulateInitial();
            Process().Forget();
        }

        private void PopulateInitial()
        {
            for (int i = 0; i < generatorConfig.InitialCount; i++)
            {
                Create();
            }
        }

        private async UniTaskVoid Process()
        {
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(generatorConfig.Delay),
                    cancellationToken: destroyCancellationToken);
                if (_obstacles.Count >= generatorConfig.MaxCount)
                {
                    continue;
                }

                Create();
            }
        }

        private void Create()
        {
            Obstacle obstacle = factory.Create();
            if (obstacle == null)
            {
                return;
            }

            obstacle.OnDestroyAsObservable().Subscribe(_ => _obstacles.Remove(obstacle)).AddTo(this);
            _obstacles.Add(obstacle);
        }
    }
}