using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game.Character.Elf.TaskManager
{
    public class LootingTask : ITask
    {
        private readonly ILootable _lootable;
        private readonly ICollectible _collectible;

        public LootingTask(ILootable lootable,ICollectible collectible)
        {
            _lootable = lootable;
            _collectible = collectible;
        }

        public UniTask Execute()
        {
            return _lootable.Loot(_collectible);
        }
    }
}