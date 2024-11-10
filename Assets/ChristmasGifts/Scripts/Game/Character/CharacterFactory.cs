using System.Collections.Generic;
using System.Linq;
using ChristmasGifts.Scripts.Config.Characters;
using ChristmasGifts.Scripts.Game.Tools;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Character
{
    public class CharacterFactory : MonoBehaviour
    {
        [SerializeField] private List<CharacterConfig> characters;
        [SerializeField] private GridPlacer gridPlacer;

        public List<TCharacter> Populate<TCharacter, TCharacterConfig>()
            where TCharacter : AbstractCharacter<TCharacterConfig>
            where TCharacterConfig : CharacterConfig<TCharacter>
        {
            TCharacterConfig config = characters.Find(config => config is TCharacterConfig) as TCharacterConfig;
            if (config == null)
            {
                Debug.LogError($"Unable to find config for character type: {typeof(TCharacter)}");
                return null;
            }

            return gridPlacer
                .GetPositions()
                .Select(position => Create<TCharacter, TCharacterConfig>(config, position))
                .ToList();

        }

        private TCharacter Create<TCharacter, TCharacterConfig>(TCharacterConfig characterConfig, Vector3 position)
            where TCharacter : AbstractCharacter<TCharacterConfig>
            where TCharacterConfig : CharacterConfig<TCharacter>
        {
            TCharacter instance = Instantiate(characterConfig.CharacterPrefab, gridPlacer.transform);
            instance.name = $"{typeof(TCharacter)} {gridPlacer.transform.childCount}";
            instance.transform.SetPositionAndRotation(position, Quaternion.identity);
            instance.Setup(characterConfig);
            return instance;
        }
    }
}