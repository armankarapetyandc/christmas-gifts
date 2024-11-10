using UniRx;

namespace ChristmasGifts.Scripts.Game.StateMachine
{
    public enum StateStatus
    {
        None,
        Entering,
        Entered,
        Updating,
        Exiting,
        Exited
    }

    public class State : IState
    {
        protected readonly StateMachine StateMachine;
        protected bool HasEntered { get; private set; }

        private readonly ReactiveProperty<StateStatus> _stateStatusProperty =
            new ReactiveProperty<StateStatus>(StateStatus.None);

        public IReadOnlyReactiveProperty<StateStatus> StateStatusProperty => _stateStatusProperty;

        protected State(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        void IState.Enter()
        {
            HasEntered = true;
            _stateStatusProperty.Value = StateStatus.Entering;
            OnEnter();
            _stateStatusProperty.Value = StateStatus.Entered;
        }

        void IState.Update()
        {
            if (!HasEntered)
            {
                return;
            }

            _stateStatusProperty.Value = StateStatus.Updating;
            OnUpdate();
        }

        void IState.Exit()
        {
            _stateStatusProperty.Value = StateStatus.Exiting;
            OnExit();
            _stateStatusProperty.Value = StateStatus.Exited;
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnExit()
        {
        }
    }

    public class State<TProperties> : State
    {
        protected TProperties Properties { get; }

        protected State(StateMachine stateMachine, TProperties properties) : base(stateMachine)
        {
            Properties = properties;
        }
    }
}