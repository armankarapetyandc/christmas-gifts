using System;
using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChristmasGifts.Scripts.Game.Prize
{
    public class PrizeFactory : MonoBehaviour
    {
        [SerializeField] private List<PrizeConfig> prizeConfigs;
        [SerializeField] private Transform container;
        [SerializeField] private NavMeshSpawner navMeshSpawner;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Create();
            }
        }

        public void Create()
        {
            PrizeConfig randomPrizeConfig = prizeConfigs[Random.Range(0, prizeConfigs.Count)];
            if (navMeshSpawner.TrySpawn(randomPrizeConfig.Prefab, container, Quaternion.identity, out Prize instance))
            {
                instance.Setup(randomPrizeConfig);
            }
        }
    }
}