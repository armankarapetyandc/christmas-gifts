using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Obstacles
{
    [CreateAssetMenu(fileName = "Obstacle Generator Config", menuName = "ChristmasGifts/Obstacles/Obstacle Generator Config",
        order = 1)]
    public class ObstacleGeneratorConfig : ScriptableObject
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }
    }
}