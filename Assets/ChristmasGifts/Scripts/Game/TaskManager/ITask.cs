using System.Threading;
using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public interface ITask
    {
        UniTask Execute();
    }
}