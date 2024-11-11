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
            if (collectible == null)
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

            var ordered = calculated.OrderBy(pair => pair.Value);
            
            var optimal = ordered.Select(pair => pair.Key).FirstOrDefault();
            if (optimal == null)
            {
                throw new Exception("Unable to give task to the optimal character");
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