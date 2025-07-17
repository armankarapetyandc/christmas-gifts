using UnityEngine;

namespace SpaceMonkey.Scripts.Installers.Bootstrap
{
    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "Space Monkey/Bootstrap Config")]
    public class BootstrapConfig : ScriptableObject
    {
        [field: SerializeField] public ushort TargetFrameRate { get; private set; }
        [field: SerializeField] public string MainSceneName { get; private set; }
    }
}