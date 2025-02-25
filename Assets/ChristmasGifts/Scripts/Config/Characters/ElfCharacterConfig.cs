using ChristmasGifts.Scripts.Game.Character;
using ChristmasGifts.Scripts.Game.Character.Elf;
using UnityEngine;

namespace ChristmasGifts.Scripts.Config.Characters
{
    [CreateAssetMenu(fileName = "Elf Character Config", menuName = "ChristmasGifts/Characters/Elf Character Config", order = 1)]
    public class ElfCharacterConfig : CharacterConfig<ElfCharacter>
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float StoppingDistance { get; private set; }
    }
}