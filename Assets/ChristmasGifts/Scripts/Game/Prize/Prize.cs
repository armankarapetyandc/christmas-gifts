using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Prize
{
    public class Prize : MonoBehaviour, ICollectible
    {
        [SerializeField] private float lootingTime;

        public async UniTask Collect()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lootingTime));
            Destroy(gameObject);
        }
    }
}