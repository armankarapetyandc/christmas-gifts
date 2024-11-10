using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.Character.Elf.StateMachine;
using ChristmasGifts.Scripts.Game.Character.Elf.TaskManager;
using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ChristmasGifts.Scripts.Game.Character.Elf
{
    public class ElfCharacter :
        AbstractCharacter<ElfStateMachine, ElfStateMachineFactory, ElfCharacter, ElfCharacterConfig>, IMoveable,
        ILootable
    {
        [SerializeField] private NavMeshAgent agent;
        internal NavMeshAgent Agent => agent;

        public bool ShouldCollect => TaskManager.ShouldCollect;


        protected override void InternalStart()
        {
            agent.speed = CharacterConfig.MovementSpeed;
            agent.stoppingDistance = CharacterConfig.StoppingDistance;
            TaskManager.Run(destroyCancellationToken);
        }

        UniTask IMoveable.Move(Vector3 destinationPosition)
        {
            return StateMachineFactory.UseMovement(destinationPosition);
        }

        UniTask ILootable.Loot(ICollectible collectible)
        {
            return StateMachineFactory.UseLooting(collectible);
        }

        public void DoJob(ICollectible collectible, Vector3 hitPoint)
        {
            TaskManager.Enqueue(new MovementTask(this, hitPoint));
            if (collectible != null)
            {
                TaskManager.Enqueue(new LootingTask(this, collectible));
            }
        }
    }
}