using ChristmasGifts.Scripts.Game.Character;
using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Characters
{
    public abstract class CharacterConfig : ScriptableObject
    {
    }

    public abstract class CharacterConfig<TCharacter> : CharacterConfig where TCharacter : AbstractCharacter
    {
        [field: SerializeField] public TCharacter CharacterPrefab { get; private set; }
    }
}