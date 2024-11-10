using UnityEngine;

namespace ChristmasGifts.Scripts.Game.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;

        private void Update()
        {
            ((IState)_currentState)?.Update();
        }

        public void ChangeState(State state)
        {
            Debug.Log($"[StateMachine] Change State: Exiting Current: {_currentState} -> Will Enter: {state}");
            ((IState)_currentState)?.Exit();
            _currentState = state;
            Debug.Log($"[StateMachine] Change State: Entering: {_currentState}");
            ((IState)_currentState).Enter();
        }
    }
}