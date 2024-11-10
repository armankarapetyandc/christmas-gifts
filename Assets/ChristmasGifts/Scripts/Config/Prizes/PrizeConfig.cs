using ChristmasGifts.Scripts.Game.Prize;
using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Prizes
{
    [CreateAssetMenu(fileName = "Prize Config", menuName = "ChristmasGifts/Prizes/Prize Config", order = 1)]
    public class PrizeConfig : ScriptableObject
    {
        [field:SerializeField]public Prize Prefab { get; private set; }
        [field:SerializeField]public float LootingTime { get; private set; }
    }
}