using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class IdeaInfo
    {
        public string Name { get; }
        public Sprite Icon { get; }

        public IdeaInfo(string name, Sprite icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}