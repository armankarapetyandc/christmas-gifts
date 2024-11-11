using System;
using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Config.Prizes;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Prize
{
    public class PrizeGenerator : MonoBehaviour, IGameRun
    {
        [SerializeField] private PrizeGeneratorConfig generatorConfig;
        [SerializeField] private PrizeFactory factory;

        private readonly List<Prize> _prizes = new List<Prize>();

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
                if (_prizes.Count >= generatorConfig.MaxCount)
                {
                    continue;
                }

                Create();
            }
        }

        private void Create()
        {
            Prize prize = factory.Create();
            if (prize == null)
            {
                return;
            }

            prize.OnDestroyAsObservable().Subscribe(_ => _prizes.Remove(prize)).AddTo(this);
            _prizes.Add(prize);
        }
    }
}