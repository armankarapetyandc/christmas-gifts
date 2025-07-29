using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public class IconBuilderTab : MonoBehaviour
    {
        [SerializeField] private List<IconItem> iconItems;
        [SerializeField] private List<ColorItem> colorItems;

        public Observable<Sprite> IconSelected => iconItems.Select(item => item.OnSelected).Merge();
        public Observable<Color> ColorSelected => colorItems.Select(item => item.OnSelected).Merge();

        public void Initialize(List<Sprite> icons, List<Color> colors)
        {
            for (var i = 0; i < iconItems.Count; ++i)
            {
                iconItems[i].Set(icons[i]);
            }

            for (var i = 0; i < colorItems.Count; ++i)
            {
                colorItems[i].Set(colors[i]);
            }
        }
    }
}