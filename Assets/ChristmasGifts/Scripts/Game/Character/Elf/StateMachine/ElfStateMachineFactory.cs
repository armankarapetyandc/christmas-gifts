using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfStateMachineFactory
    {
        private readonly ElfStateMachine _stateMachine;
        private readonly ElfCharacter _elfCharacter;

        public ElfStateMachineFactory(ElfStateMachine stateMachine, ElfCharacter elfCharacter)
        {
            _stateMachine = stateMachine;
            _elfCharacter = elfCharacter;
        }

        public UniTask UseMovement(Vector3 destinationPosition)
        {
            ElfMovementState state = new ElfMovementState(_stateMachine,
                new ElfMovementState.Props(_elfCharacter.Agent, destinationPosition));
            _stateMachine.ChangeState(state);
            return state.AwaitCompletion();
        }

        public UniTask UseLooting(ICollectible collectible)
        {
            ElfLootingState state = new ElfLootingState(_stateMachine, new ElfLootingState.Props(collectible));
            _stateMachine.ChangeState(state);
            return state.AwaitCompletion();
        }

        public void UseIdle()
        {
            _stateMachine.ChangeState(new ElfIdleState(_stateMachine));
        }
    }
}