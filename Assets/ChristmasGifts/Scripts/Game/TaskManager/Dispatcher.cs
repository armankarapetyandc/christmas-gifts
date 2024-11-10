using System.Collections.Generic;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.Character;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public class Dispatcher : MonoBehaviour
    {
        [SerializeField] private CharacterFactory characterFactory;

        private List<ElfCharacter> _elfCharacters;

        private void Start()
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
                    Debug.LogError(
                        $"{character}: ShouldCollect: {character.ShouldCollect} Do Collectible: {collectible}");
                    character.DoJob(collectible, hitPoint);
                    collectibleTaskGiven = true;
                }

                character.DoJob(null, hitPoint);
            }
        }
    }
}