using UnityEngine;

namespace SpaceMonkey.Scripts.Configs.Map
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Space Monkey/Configs/Map Config", order = 1)]
    public class MapConfig : ScriptableObject
    {
        [field: SerializeField] public MapPlace[] Places { get; private set; }
    }
}