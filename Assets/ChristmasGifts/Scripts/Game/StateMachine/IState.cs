namespace ChristmasGifts.Scripts.Game.StateMachine
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}