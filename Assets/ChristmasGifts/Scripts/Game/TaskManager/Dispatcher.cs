using System;
using System.Collections.Generic;
using System.Linq;
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
            /*bool collectibleTaskGiven = false;
            foreach (var character in _elfCharacters)
            {
                if (!character.ShouldCollect && !collectibleTaskGiven && collectible != null)
                {
                    character.DoJob(collectible, hitPoint);
                    collectibleTaskGiven = true;
                }

                character.DoJob(null, hitPoint);
            }*/


            if (collectible==null)
            {
                foreach (var character in _elfCharacters)
                {
                    character.DoJob(hitPoint, null);
                }
                return;
            }
            
            var currentSnapshot = GetSnapshot();
            var calculated = new Dictionary<ElfCharacter, float>();
            foreach (var pair in currentSnapshot)
            {
                if (pair.Key.TryEvaluateTimeToDestination(hitPoint, out float timeToDestination))
                {
                    calculated.Add(pair.Key, pair.Value + timeToDestination);
                }
            }

            var optimal = calculated.OrderBy(pair => pair.Value).Select(pair => pair.Key).FirstOrDefault();
            if (optimal == null)
            {
                throw new Exception("Unable to give task to characters");
            }

            optimal.DoJob(hitPoint, collectible);
        }


        private Dictionary<ElfCharacter, float> GetSnapshot()
        {
            var durationsSnapshot = new Dictionary<ElfCharacter, float>();
            foreach (ElfCharacter elfCharacter in _elfCharacters)
            {
                var remainingTime = elfCharacter.TaskManager.EvaluateTasksRemainingTime();
                durationsSnapshot.Add(elfCharacter, remainingTime);
            }

            return durationsSnapshot;
        }
    }
}