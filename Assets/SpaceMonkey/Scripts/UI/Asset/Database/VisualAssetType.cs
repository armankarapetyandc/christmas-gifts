using System;

namespace SpaceMonkey.Scripts.UI.Asset.Database
{
    [Flags]
    public enum VisualAssetType
    {
        None = 0,
        Generic = 1 << 0, // 1
        Company = 1 << 1, // 2
        Product = 1 << 2, // 4
        Customer = 1 << 3, // 8
        Bust = 1 << 4
    }
}