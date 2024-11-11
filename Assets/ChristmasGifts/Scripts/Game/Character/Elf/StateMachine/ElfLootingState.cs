using ChristmasGifts.Scripts.Game.StateMachine;
using ChristmasGifts.Scripts.Game.TaskManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfLootingState : State<ElfLootingState.Props>, IStateProgress
    {
        public class Props
        {
            public ICollectible Collectible { get; }

            public Props(ICollectible collectible)
            {
                Collectible = collectible;
            }
        }

        public float RemaniningTime { get; private set; }

        public ElfLootingState(Game.StateMachine.StateMachine stateMachine, Props properties) : base(stateMachine,
            properties)
        {
        }


        public async UniTask AwaitCompletion()
        {
            RemaniningTime = Properties.Collectible.Duration;

            while (RemaniningTime > 0f)
            {
                RemaniningTime -= Time.deltaTime;
                await UniTask.Yield();
            }

            RemaniningTime = 0f;
            Properties.Collectible.Collect();
            StateMachine.ChangeState(new ElfIdleState(StateMachine));
        }
    }
}