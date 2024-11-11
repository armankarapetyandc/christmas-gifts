using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public interface ITask
    {
        TaskStatus Status { get; }
        Vector3 Position { get; }
        float RemainingTime { get; }
        UniTask Execute();
    }

    public enum TaskStatus
    {
        None,
        Executing,
        Completed
    }
}