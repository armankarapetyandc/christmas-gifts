using System;
using ChristmasGifts.Scripts.Game.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfMovementState : State<ElfMovementState.Props>
    {
        public class Props
        {
            public ElfCharacter Character { get; }
            public Vector3 DestinationPosition { get; }

            public Props(ElfCharacter character, Vector3 destinationPosition)
            {
                Character = character;
                DestinationPosition = destinationPosition;
            }
        }

        private bool _hasReachedDestination;
        

        private UniTaskCompletionSource _completionSource;


        public ElfMovementState(Game.StateMachine.StateMachine stateMachine, Props properties) : base(
            stateMachine, properties)
        {
        }

        protected override void OnEnter()
        {
            _completionSource = new UniTaskCompletionSource();
            Debug.Log($"[ElfMovementState] OnEnter: DestinationPosition: {Properties.DestinationPosition}");
            _hasReachedDestination = false;
            Properties.Character.Agent.SetDestination(Properties.DestinationPosition);
        }

        protected override void OnUpdate()
        {
            // Only proceed if the agent has a valid and complete path
            if (Properties.Character.Agent.pathPending ||
                Properties.Character.Agent.pathStatus != NavMeshPathStatus.PathComplete)
                return;

            // Check if the agent has reached the destination
            if (!_hasReachedDestination && IsAgentAtDestination())
            {
                _hasReachedDestination = true;
                OnDestinationReached();
            }
            else if (_hasReachedDestination && !IsAgentAtDestination())
            {
                // Reset the flag if the agent starts moving again
                _hasReachedDestination = false;
            }
        }

        private bool IsAgentAtDestination()
        {
            // Check if the agent is within the stopping distance and has stopped moving
            return Properties.Character.Agent.remainingDistance <= Properties.Character.Agent.stoppingDistance &&
                   Properties.Character.Agent.velocity.sqrMagnitude == 0f;
        }

        private void OnDestinationReached()
        {
            Debug.Log($"Elf has reached the destination.: {Properties.DestinationPosition}");
            StateMachine.ChangeState(new ElfIdleState(StateMachine));
            _completionSource.TrySetResult();
        }


        public UniTask AwaitCompletion()
        {
            return _completionSource.Task;
        }

        protected override void OnExit()
        {
            if (!_hasReachedDestination)
            {
                _completionSource.TrySetCanceled();
            }

            Debug.Log($"[ElfMovementState] OnExit: {Properties.DestinationPosition}");
        }
    }
}