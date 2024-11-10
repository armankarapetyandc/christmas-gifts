using ChristmasGifts.Scripts.Game.TaskManager;
using UniRx;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game
{
    public class PlayerController : MonoBehaviour, IGameRun
    {
        [SerializeField] private Dispatcher dispatcher;
        [SerializeField] private Pointer pointer;

        private bool _isRunning;

        private void Start()
        {
            pointer.PointerClickStream.Subscribe(OnPointerClickStream).AddTo(this);
        }

        public void Run()
        {
            dispatcher.PopulateCharacters();
            _isRunning = true;
        }

        private void OnPointerClickStream((ICollectible collectible, Vector3 hitPoint) tuple)
        {
            if (!_isRunning)
            {
                return;
            }

            dispatcher.DoJob(tuple.collectible, tuple.hitPoint);
        }
    }
}