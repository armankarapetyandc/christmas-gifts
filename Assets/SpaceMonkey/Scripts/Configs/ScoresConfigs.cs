using System;
using System.Collections.Generic;
using System.Linq;
using SpaceMonkey.Scripts.Configs.Scores;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ScoresConfigs", menuName = "Space Monkey/Configs/Scores/Scores Configs", order = 0)]
    public class ScoresConfigs : ScriptableObject
    {
        [SerializeField] private List<ScoreConfig> scoresConfigs;

        public int CalculateScoreConfigByKey(string key)
        {
            ScoreConfig scoreConfig = scoresConfigs.FirstOrDefault(config =>
                string.Equals(config.Key, key, StringComparison.OrdinalIgnoreCase));
         
            if (scoreConfig != null)
            {
                if (scoreConfig.AvailableActionsCount == -1)
                {
                    return scoreConfig.Score;
                }
                int availableActionsCount = PlayerPrefs.GetInt(key,scoreConfig.AvailableActionsCount);
                if (availableActionsCount <= 0)
                {
                    return 0;
                }

                availableActionsCount--;
                PlayerPrefs.SetInt(key, availableActionsCount);
                return scoreConfig.Score;
            }

            return 0;
        }

        public int PeekScoreConfigByKey(string key)
        {
            ScoreConfig scoreConfig = scoresConfigs.FirstOrDefault(config =>
                string.Equals(config.Key, key, StringComparison.OrdinalIgnoreCase));
            return scoreConfig!=null ? scoreConfig.Score : 0;
        }
    }
}