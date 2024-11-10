using ChristmasGifts.Scripts.Game.StateMachine;
using Cysharp.Threading.Tasks;

namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfLootingState : State<ElfLootingState.Props>
    {
        public class Props
        {
            public ICollectible Collectible { get; }

            public Props(ICollectible collectible)
            {
                Collectible = collectible;
            }
        }

        public ElfLootingState(Game.StateMachine.StateMachine stateMachine, Props properties) : base(stateMachine,
            properties)
        {
        }

        public async UniTask AwaitCompletion()
        {
            await Properties.Collectible.Collect();
            StateMachine.ChangeState(new ElfIdleState(StateMachine));
        }
    }
}