using System.Collections.Generic;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Asset.IconBuilder
{
    [CreateAssetMenu(fileName = "IconBuilder", menuName = "Space Monkey/Icon Builder Config")]
    public class IconBuilderConfig : ScriptableObject
    {
        [SerializeField] public List<Sprite> ShapesSprites;
        [SerializeField] public List<Sprite> IconSprites;
        [SerializeField] public List<Color> BackgroundColors;
    }
}