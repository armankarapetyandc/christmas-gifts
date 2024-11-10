using UnityEngine;

namespace ChristmasGifts.Scripts.Config
{
    [CreateAssetMenu(fileName = "Bootstrap Config", menuName = "ChristmasGifts/Bootstrap Config", order = 1)]
    public class BootstrapConfig : ScriptableObject
    {
        [field: SerializeField] public string MainSceneName { get; private set; }
        [field: SerializeField] public int TargetFrameRate { get; private set; }
    }
}