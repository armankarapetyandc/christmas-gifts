using System;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.Character.Elf.StateMachine;
using ChristmasGifts.Scripts.Game.Character.Elf.Tasks;
using ChristmasGifts.Scripts.Game.TaskManager;
using UnityEngine;
using UnityEngine.AI;

namespace ChristmasGifts.Scripts.Game.Character.Elf
{
    public class ElfCharacter :
        AbstractCharacter<ElfStateMachine, ElfStateMachineFactory, ElfCharacter, ElfCharacterConfig>
    {
        [SerializeField] private NavMeshAgent agent;
        internal NavMeshAgent Agent => agent;

        public bool IsBusy => !StateMachineFactory.StateMachine.IsState<ElfIdleState>();


        protected override void InternalStart()
        {
            agent.speed = CharacterConfig.MovementSpeed;
            agent.stoppingDistance = CharacterConfig.StoppingDistance;
            TaskManager.Run(destroyCancellationToken);
        }

        internal bool TryEvaluateRemainingDistance(Vector3 position, out float distance)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                float totalDistance = 0;

                // Calculate the distance along the path
                for (int i = 1; i < path.corners.Length; i++)
                {
                    totalDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                distance = totalDistance;
                return true;
            }

            distance = -1f;
            return false;
        }

        internal bool TryEvaluateRemainingDistance(Vector3 fromPosition, Vector3 toPosition, out float distance)
        {
            NavMeshPath path = new NavMeshPath();

            if (NavMesh.CalculatePath(fromPosition, toPosition, NavMesh.AllAreas, path) &&
                path.status == NavMeshPathStatus.PathComplete)
            {
                float totalDistance = 0;

                // Calculate the distance along the path
                for (int i = 1; i < path.corners.Length; i++)
                {
                    totalDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                distance = totalDistance;
                return true;
            }

            distance = -1f;
            return false;
        }

        internal bool TryEvaluateTimeToDestination(Vector3 position, out float timeToDestination)
        {
            if (!TryEvaluateRemainingDistance(position, out float distance))
            {
                timeToDestination = Mathf.Infinity;
                return false;
            }

            timeToDestination = distance / agent.speed;
            return true;
        }

        internal bool TryEvaluateTimeToDestination(Vector3 fromPosition, Vector3 toPosition,
            out float timeToDestination)
        {
            if (!TryEvaluateRemainingDistance(fromPosition, toPosition, out float distance))
            {
                timeToDestination = Mathf.Infinity;
                return false;
            }

            timeToDestination = distance / agent.speed;
            return true;
        }


        public void DoJob(Vector3 destinationPosition, ICollectible collectible)
        {
            bool canMove = DoMovementJob(destinationPosition);
            if (collectible == null || !canMove)
            {
                return;
            }

            CollectionTask task = new CollectionTask(() =>
                {
                    if (StateMachineFactory.StateMachine.IsState<ElfLootingState>())
                    {
                        return StateMachineFactory.StateMachine.GetState<ElfLootingState>().RemaniningTime;
                    }

                    return collectible.Duration;
                }, () => StateMachineFactory.UseLooting(collectible),
                destinationPosition);

            TaskManager.Enqueue(task);
        }

        private bool DoMovementJob(Vector3 destinationPosition)
        {
            try
            {
                ITask previousTask = TaskManager.GetPreviousTask();
                MovementTask task = new MovementTask(
                    () =>
                    {
                        float timeToDestination;

                        if (previousTask == null)
                        {
                            if (!TryEvaluateTimeToDestination(destinationPosition, out timeToDestination))
                            {
                                throw new Exception(
                                    $"Unable to evaluate time to given destination: Position: {destinationPosition}");
                            }
                        }
                        else
                        {
                            if (!TryEvaluateTimeToDestination(previousTask.Position, destinationPosition,
                                    out timeToDestination))
                            {
                                throw new Exception(
                                    $"Unable to evaluate time to given destination: Position: {destinationPosition}");
                            }
                        }

                        return timeToDestination;
                    },
                    () => StateMachineFactory.UseMovement(destinationPosition),
                    destinationPosition
                );

                task.Validate();
                TaskManager.Enqueue(task);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Warning: {e}");
                return false;
            }
        }
    }
}