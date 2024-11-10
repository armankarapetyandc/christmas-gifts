using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.Character;
using ChristmasGifts.Scripts.Game.Character.Elf;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public class Dispatcher : MonoBehaviour
    {
        [SerializeField] private CharacterFactory characterFactory;

        private List<ElfCharacter> _elfCharacters;

        public void PopulateCharacters()
        {
            _elfCharacters = characterFactory.Populate<ElfCharacter, ElfCharacterConfig>();
        }

        public void DoJob(ICollectible collectible, Vector3 hitPoint)
        {
            bool collectibleTaskGiven = false;
            foreach (var character in _elfCharacters)
            {
                if (!character.ShouldCollect && !collectibleTaskGiven && collectible != null)
                {
                    character.DoJob(collectible, hitPoint);
                    collectibleTaskGiven = true;
                }

                character.DoJob(null, hitPoint);
            }
        }
    }
}