using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game
{
    public interface ICollectible
    {
        UniTask Collect();
    }
}