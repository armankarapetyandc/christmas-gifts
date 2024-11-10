using System;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Config.Prizes;
using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Prize
{
    public class Prize : MonoBehaviour, ICollectible
    {
        private PrizeConfig _config;
        public void Setup(PrizeConfig config)
        {
            _config = config;
        }

        public async UniTask Collect()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.LootingTime));
            Destroy(gameObject);
        }
    }
}