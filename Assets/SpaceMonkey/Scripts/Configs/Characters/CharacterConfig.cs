using SpaceMonkey.Scripts.UI.Asset.Database;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs.Characters
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Space Monkey/Configs/Character Config", order = 1)]
    public class CharacterConfig : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public SpriteVisualAsset Sprite { get; private set; }
        [field: SerializeField] public CharacterMoodVisualAsset[] HeadVisualAssets { get; private set; }
        [field: SerializeField] public SpriteVisualAsset FullBodySprite { get; private set; }
        [field: SerializeField] public ColorVisualAsset BackgroundColor { get; private set; }
        [field: SerializeField] public float MoodMin { get; private set; }
        [field: SerializeField] public float MoodMax { get; private set; }
        [field: SerializeField] public float DefaultMood { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Id = Name.ToLower().Replace(" ", "_").Replace(".", "");
            DefaultMood = Mathf.Clamp(DefaultMood, MoodMin, MoodMax);
        }
#endif
    }
}