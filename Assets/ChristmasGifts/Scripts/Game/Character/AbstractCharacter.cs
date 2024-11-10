using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.StateMachine;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character
{
    public abstract class AbstractCharacter : MonoBehaviour
    {
        [SerializeField] private Game.TaskManager.TaskManager taskManager;

        protected Game.TaskManager.TaskManager TaskManager => taskManager;

        protected virtual void Start()
        {
            InternalStart();
        }

        protected virtual void InternalStart()
        {
        }
    }

    public abstract class AbstractCharacter<TCharacterConfig> : AbstractCharacter
        where TCharacterConfig : CharacterConfig
    {
        protected TCharacterConfig CharacterConfig { get; private set; }

        public void Setup(TCharacterConfig characterConfig)
        {
            CharacterConfig = characterConfig;
        }
    }

    public abstract class
        AbstractCharacter<TStateMachine, TStateMachineFactory, TCharacter,
            TCharacterConfig> : AbstractCharacter<TCharacterConfig>
        where TStateMachine : StateMachine.StateMachine
        where TStateMachineFactory : StateMachineFactory<TStateMachine, TCharacter>, new()
        where TCharacterConfig : CharacterConfig
        where TCharacter : AbstractCharacter<TStateMachine, TStateMachineFactory, TCharacter, TCharacterConfig>
    {
        [SerializeField] private TStateMachine stateMachine;
        
        private TStateMachineFactory _stateMachineFactory;

        protected TStateMachineFactory StateMachineFactory => _stateMachineFactory;


        protected sealed override void Start()
        {
            _stateMachineFactory = new TStateMachineFactory
            {
                StateMachine = stateMachine,
                Character = (TCharacter)this
            };
            base.Start();
        }
    }
}