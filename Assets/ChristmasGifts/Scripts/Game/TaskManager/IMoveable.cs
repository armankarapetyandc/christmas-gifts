using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public interface IMoveable
    {
        UniTask Move(Vector3 destinationPosition);
    }
}