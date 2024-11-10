using ChristmasGifts.Scripts.Game.Obstacle;
using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Obstacles
{
    [CreateAssetMenu(fileName = "Obstacle Config", menuName = "ChristmasGifts/Obstacles/Obstacle Config", order = 1)]
    public class ObstacleConfig : ScriptableObject
    {
        [field: SerializeField] public Obstacle Prefab { get; private set; }
    }
}