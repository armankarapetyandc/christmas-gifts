using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Prizes
{
    [CreateAssetMenu(fileName = "Prize Generator Config", menuName = "ChristmasGifts/Prizes/Prize Generator Config",
        order = 1)]
    public class PrizeGeneratorConfig : ScriptableObject
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }
    }
}