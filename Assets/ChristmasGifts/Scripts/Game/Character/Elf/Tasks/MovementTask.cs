using System;
using ChristmasGifts.Scripts.Game.StateMachine;
using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.Elf.Tasks
{
    public class MovementTask : ITask
    {
        private readonly Func<float> _timeToDestinationCallback;
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

                return _timeToDestinationCallback();
            }
        }


        public MovementTask(Func<float> timeToDestinationCallback, Func<UniTask> actualJob, Vector3 position)
        {
            _timeToDestinationCallback = timeToDestinationCallback;
            _actualJob = actualJob;
            Position = position;
            Status = TaskStatus.None;
        }

        public void Validate()
        {
            var time = _timeToDestinationCallback();
        }

        public async UniTask Execute()
        {
            Status = TaskStatus.Executing;
            await _actualJob();
            Status = TaskStatus.Completed;
        }
    }
}