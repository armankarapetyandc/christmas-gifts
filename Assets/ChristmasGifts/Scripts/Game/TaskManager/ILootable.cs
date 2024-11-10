using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public interface ILootable
    {
        UniTask Loot(ICollectible collectible);
    }
}