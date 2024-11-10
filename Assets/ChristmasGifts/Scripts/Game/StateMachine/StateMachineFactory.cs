using ChristmasGifts.Scripts.Game.Character;

namespace ChristmasGifts.Scripts.Game.StateMachine
{
    public class StateMachineFactory<TStateMachine, TCharacter>
        where TStateMachine : StateMachine
        where TCharacter : AbstractCharacter
    {
        internal TStateMachine StateMachine;
        internal TCharacter Character;
    }
}