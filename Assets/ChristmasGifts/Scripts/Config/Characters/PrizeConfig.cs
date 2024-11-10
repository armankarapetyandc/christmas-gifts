using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Characters
{
    [CreateAssetMenu(fileName = "Prize Config", menuName = "ChristmasGifts/Prize Config", order = 1)]
    public class PrizeConfig : ScriptableObject
    {
        [field:SerializeField]public float LootingTime { get; private set; }
    }
}