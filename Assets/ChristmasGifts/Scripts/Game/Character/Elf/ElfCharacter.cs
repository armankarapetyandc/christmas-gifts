using System;
using ChristmasGifts.Scripts.Game.Character.Elf.StateMachine;
using ChristmasGifts.Scripts.Game.Character.Elf.TaskManager;
using ChristmasGifts.Scripts.Game.Character.TaskManager;
using ChristmasGifts.Scripts.Game.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ChristmasGifts.Scripts.Game.Character
{
    public class ElfCharacter : MonoBehaviour, IMoveable, ILootable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float stoppingDistance;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private ElfStateMachine stateMachine;
        [SerializeField] private Game.TaskManager.TaskManager taskManager;
        [field: SerializeField] public bool IsMain { get; private set; }
        internal NavMeshAgent Agent => agent;

        private ElfStateMachineFactory _stateMachineFactory;

        public bool ShouldCollect => taskManager.ShouldCollect;


        private void Start()
        {
            _stateMachineFactory = new ElfStateMachineFactory(stateMachine, this);
            agent.speed = movementSpeed;
            agent.stoppingDistance = stoppingDistance;
            taskManager.Run(destroyCancellationToken);
        }

        async UniTask IMoveable.Move(Vector3 destinationPosition)
        {
            // IsBusy = true;
            await _stateMachineFactory.UseMovement(destinationPosition);
            // IsBusy = false;
        }

        async UniTask ILootable.Loot(ICollectible collectible)
        {
            // IsBusy = true;
            await _stateMachineFactory.UseLooting(collectible);
            // IsBusy = false;
        }

        public void DoJob(ICollectible collectible, Vector3 hitPoint)
        {
            taskManager.Enqueue(new MovementTask(this, hitPoint));
            if (collectible != null)
            {
                taskManager.Enqueue(new LootingTask(this, collectible));
            }
        }
    }
}