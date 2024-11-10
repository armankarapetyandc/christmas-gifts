using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game
{
    public interface IMoveable
    {
        UniTask Move(Vector3 destinationPosition);
    }
}