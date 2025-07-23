using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.CategorySelection.BusinessIdeaItems
{
    public class BusinessItemData
    {
        public string BusinessName { get; private set; }
        public Sprite BusinessIcon { get; private set; }

        public BusinessItemData(string businessName, Sprite businessIcon)
        {
            BusinessName = businessName;
            BusinessIcon = businessIcon;
        }
    }
}