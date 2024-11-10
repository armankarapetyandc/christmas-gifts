using ChristmasGifts.Scripts.Game.StateMachine;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfIdleState : State
    {
        public ElfIdleState(Game.StateMachine.StateMachine stateMachine) : base(stateMachine)
        {
        }

        protected override void OnEnter()
        {
            Debug.Log("[ElfIdleState] OnEnter");
        }

        protected override void OnExit()
        {
            Debug.Log("[ElfIdleState] OnExit");
        }
    }
}