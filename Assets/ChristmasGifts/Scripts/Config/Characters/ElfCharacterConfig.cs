using ChristmasGifts.Scripts.Game.Character;
using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Characters
{
    [CreateAssetMenu(fileName = "Elf Character Config", menuName = "ChristmasGifts/Elf Character Config", order = 1)]
    public class ElfCharacterConfig : CharacterConfig<ElfCharacter>
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float StoppingDistance { get; private set; }
    }
}