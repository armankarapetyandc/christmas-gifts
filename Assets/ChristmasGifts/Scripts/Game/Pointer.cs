using System;
using ChristmasGifts.Scripts.Common;
using ChristmasGifts.Scripts.Game.TaskManager;
using UniRx;
using UnityEngine;
using Zenject;

namespace ChristmasGifts.Scripts.Game
{
    public class Pointer : MonoBehaviour, IGameRun
    {
        [Inject(Id = "MainCamera")] private UnityEngine.Camera mainCamera;

        [SerializeField] private LayerMask interactionLayers;
        [SerializeField] private LayerMask obstacleLayers;

        private readonly Subject<(ICollectible collectible, Vector3 hitPoint)> _pointerClickSubject =
            new Subject<(ICollectible collectible, Vector3 hitPoint)>();

        private bool _isRunning;

        public IObservable<(ICollectible collectible, Vector3 hitPoint)> PointerClickStream => _pointerClickSubject;

        public void Run()
        {
            _isRunning = true;
        }


        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0)) // Left-click
            {
                if (TryGetHitObject(out ICollectible collectible, out Vector3 hitPoint))
                {
                    Debug.Log("Hit interaction object: " + collectible + " at point " + hitPoint);
                    hitPoint.y = 0f;
                    _pointerClickSubject?.OnNext((collectible, hitPoint));
                }
                else
                {
                    Debug.Log("No valid target hit.");
                }
            }
        }

        private bool TryGetHitObject(out ICollectible collectible, out Vector3 hitPoint)
        {
            // Initialize out parameters
            collectible = null;
            hitPoint = Vector3.zero;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            // Sort hits by distance to process them from nearest to farthest
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                // Check if the hit object is an obstacle
                if (((1 << hit.collider.gameObject.layer) & obstacleLayers) != 0)
                {
                    // Hit an obstacle layer first, so cancel further processing
                    return false;
                }

                // Check if the hit object is on an interaction layer
                if (((1 << hit.collider.gameObject.layer) & interactionLayers) != 0)
                {
                    // Assign out parameters with hit information
                    collectible = Utils.FindComponentInParents<ICollectible>(hit.transform);
                    hitPoint = hit.point;
                    return true; // Return true as we have a valid interaction hit
                }
            }

            return false; // No valid hit found
        }
    }
}