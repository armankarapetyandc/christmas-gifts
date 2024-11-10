using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game
{
    public interface ILootable
    {
        UniTask Loot(ICollectible collectible);
    }
}