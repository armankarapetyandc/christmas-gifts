using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Config.Prizes;
using ChristmasGifts.Scripts.Game.Common;
using ChristmasGifts.Scripts.Game.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChristmasGifts.Scripts.Game.Prize
{
    public class PrizeFactory : ObjectFactoryNavMesh
    {
        [SerializeField] private List<PrizeConfig> prizeConfigs;

        public Prize Create()
        {
            PrizeConfig randomPrizeConfig = prizeConfigs[Random.Range(0, prizeConfigs.Count)];
            var instance = Create(randomPrizeConfig.Prefab, Quaternion.identity);
            if (instance == null)
            {
                return null;
            }

            var pos = instance.transform.position;
            pos.y = instance.transform.localScale.y / 2f;
            instance.transform.position = pos;
            instance.Setup(randomPrizeConfig);

            return instance;
        }
    }
}