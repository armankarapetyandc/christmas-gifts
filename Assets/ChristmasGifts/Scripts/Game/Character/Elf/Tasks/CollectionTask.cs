using System;
using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.Elf.Tasks
{
    public class CollectionTask : ITask
    {
        private readonly Func<float> _calculateRemainingTime;
        private readonly Func<UniTask> _actualJob;
        public TaskStatus Status { get; private set; }
        public Vector3 Position { get; }

        public float RemainingTime
        {
            get
            {
                if (Status == TaskStatus.Completed)
                {
                    return 0;
                }

                return _calculateRemainingTime();
            }
        }
        public CollectionTask(Func<float> calculateRemainingTime, Func<UniTask> actualJob, Vector3 position)
        {
            _calculateRemainingTime = calculateRemainingTime;
            _actualJob = actualJob;
            Position = position;
            Status = TaskStatus.None;
        }
        public async UniTask Execute()
        {
            Status = TaskStatus.Executing;
            await _actualJob();
            Status = TaskStatus.Completed;
        }
    }
}