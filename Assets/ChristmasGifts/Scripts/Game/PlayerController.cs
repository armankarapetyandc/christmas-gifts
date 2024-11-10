using ChristmasGifts.Scripts.Game.TaskManager;
using UniRx;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Dispatcher _dispatcher;
        [SerializeField] private Pointer pointer;

        private void Start()
        {
            pointer.PointerClickStream.Subscribe(OnPointerClickStream).AddTo(this);
        }

        private void OnPointerClickStream((ICollectible collectible, Vector3 hitPoint) tuple)
        {
            _dispatcher.DoJob(tuple.collectible, tuple.hitPoint);
        }
    }
}