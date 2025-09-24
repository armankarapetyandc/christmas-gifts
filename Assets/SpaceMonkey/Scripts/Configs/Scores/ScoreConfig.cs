using UnityEngine;

namespace SpaceMonkey.Scripts.Configs.Scores
{
    [CreateAssetMenu(fileName = "ScoreConfig", menuName = "Space Monkey/Configs/Scores/Score Config", order = 0)]
    public class ScoreConfig : ScriptableObject
    {
        [SerializeField] private string key;
        [SerializeField] private int score;
        [SerializeField] private int availableActionsCount;

        public string Key => key;
        public int Score => score;
        public int AvailableActionsCount => availableActionsCount;

        public void DecreaseAvailableActionsCount()
        {
            switch (availableActionsCount)
            {
                case -1:
                    return;
                case > 0:
                    availableActionsCount--;
                    break;
            }
        }
    }
}