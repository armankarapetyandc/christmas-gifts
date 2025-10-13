using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Utilities
{
    [Serializable]
    public struct RangeValue
    {
        [field: SerializeField] public float Min { get; private set; }
        [field: SerializeField] public float Max { get; private set; }

        public static RangeValue Create(float min, float max)
        {
            return new RangeValue
            {
                Min = min,
                Max = max
            };
        }
    }
}