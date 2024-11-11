namespace ChristmasGifts.Scripts.Game.StateMachine
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }

    public interface IStateProgress
    {
        float RemaniningTime { get; }
    }
}