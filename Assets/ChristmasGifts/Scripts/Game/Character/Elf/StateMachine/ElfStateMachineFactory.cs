using ChristmasGifts.Scripts.Game.StateMachine;
using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfStateMachineFactory : StateMachineFactory<ElfStateMachine, ElfCharacter>
    {
        public UniTask UseMovement(Vector3 destinationPosition)
        {
            ElfMovementState state = new ElfMovementState(StateMachine,
                new ElfMovementState.Props(Character, destinationPosition));
            StateMachine.ChangeState(state);
            return state.AwaitCompletion();
        }

        public UniTask UseLooting(ICollectible collectible)
        {
            ElfLootingState state = new ElfLootingState(StateMachine, new ElfLootingState.Props(collectible));
            StateMachine.ChangeState(state);
            return state.AwaitCompletion();
        }

        public void UseIdle()
        {
            StateMachine.ChangeState(new ElfIdleState(StateMachine));
        }
    }
}