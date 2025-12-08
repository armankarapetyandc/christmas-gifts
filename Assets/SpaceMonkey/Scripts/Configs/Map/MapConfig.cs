using System.Collections.Generic;
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
        
        
        internal List<IMapPlace> GetAppearedMapPlaces(IMapPlace[] places, int currentWeek)
        {
            var appearedPlaces = new List<IMapPlace>();
            
            foreach (var place in places)
            {
                if (!place.DefaultLocked)
                {
                    if ((place.AppearWeek != 0 && place.AppearWeek > currentWeek) ||
                        (place.AppearLevel != 0 &&
                         place.AppearLevel > currentWeek))
                    {
                        continue;
                    }
                    appearedPlaces.Add(place);
                }
            }
            return appearedPlaces;
        }
    }
}