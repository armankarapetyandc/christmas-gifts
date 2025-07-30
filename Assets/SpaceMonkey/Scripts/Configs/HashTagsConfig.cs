using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    [CreateAssetMenu(fileName = "HashTagsConfig", menuName = "Space Monkey/Configs/Hash Tags Config", order = 1)]
    public class HashTagsConfig : ScriptableObject
    {
        [field: SerializeField] public string[] Tags { get; set; }
    }
}