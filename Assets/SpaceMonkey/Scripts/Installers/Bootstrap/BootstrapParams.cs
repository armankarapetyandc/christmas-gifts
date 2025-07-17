using System;
using UnityEngine;

namespace SpaceMonkey.Scripts.Installers.Bootstrap
{
    [Serializable]
    public struct BootstrapParams
    {
        [field: SerializeField] public string BoostrapConfigPath { get; private set; }
    }
}