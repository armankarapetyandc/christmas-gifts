using UnityEngine;

namespace SpaceMonkey.Scripts.Configs.Map
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Space Monkey/Configs/Map Config", order = 1)]
    public class MapConfig : ScriptableObject
    {
        [field: SerializeField] public RuntimeMapPlace DefaultCompanyPlace { get; private set; }
        [field: SerializeField] public MapPlace[] Places { get; private set; }


        [ContextMenu("Set Ids")]
        private void SetIds()
        {
            for (int i = 0; i < Places.Length; i++)
            {
                Places[i].Id = i+1;
            }
        }
    }
}