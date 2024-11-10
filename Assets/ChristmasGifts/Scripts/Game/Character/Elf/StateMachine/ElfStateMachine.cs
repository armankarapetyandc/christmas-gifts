namespace ChristmasGifts.Scripts.Game.Character.Elf.StateMachine
{
    public class ElfStateMachine : Game.StateMachine.StateMachine
    {
        private void Start()
        {
            //Initial State of Elf Character
            ChangeState(new ElfIdleState(this));
        }
    }
}