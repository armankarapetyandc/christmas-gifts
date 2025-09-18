using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Utilities
{
    [Serializable]
    public struct RangeValue
    {
        [field: SerializeField] public float Min { get; private set; }
        [field: SerializeField] public float Max { get; private set; }
    }
}