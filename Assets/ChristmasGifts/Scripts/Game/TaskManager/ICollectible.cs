using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public interface ICollectible
    {
        UniTask Collect();
    }
}