
namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public interface ICollectible
    {
        float Duration { get; }
        void Collect();
    }
}