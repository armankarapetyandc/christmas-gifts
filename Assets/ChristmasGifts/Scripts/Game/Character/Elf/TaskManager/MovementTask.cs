using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.TaskManager
{
    public class MovementTask : ITask
    {
        private readonly IMoveable _moveable;
        private readonly Vector3 _destinationPosition;

        public MovementTask(IMoveable moveable, Vector3 destinationPosition)
        {
            _moveable = moveable;
            _destinationPosition = destinationPosition;
        }

        public UniTask Execute()
        {
            return _moveable.Move(_destinationPosition);
        }
    }
}