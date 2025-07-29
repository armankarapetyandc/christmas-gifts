using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels
{
    public enum TabsNavigationEnum
    {
        Icon,
        Background
    }

    public class IconBuilderTab : MonoBehaviour
    {
        [SerializeField] private Button iconTabButton;
        [SerializeField] private Button backgroundTabButton;
        
        [SerializeField] GameObject iconContainer;
        [SerializeField] GameObject backgroundContainer;
        
        [SerializeField] private List<IconItem> iconItems;
        [SerializeField] private List<BackgroundItem> backgroundImages;

        private Dictionary<TabsNavigationEnum, GameObject> _tabContents;
        private TabsNavigationEnum _currentTab;
        
        public List<IconItem> IconItems => iconItems;
        public List<BackgroundItem> BackgroundItems => backgroundImages;

        public void Initialize(List<Sprite> icons, List<Color> colors)
        {
            iconTabButton.OnClickAsObservable().Subscribe(_ => SwitchTab(TabsNavigationEnum.Icon));
            backgroundTabButton.OnClickAsObservable().Subscribe(_ => SwitchTab(TabsNavigationEnum.Background));
            _tabContents = new Dictionary<TabsNavigationEnum, GameObject>
            {
                { TabsNavigationEnum.Icon, iconContainer },
                { TabsNavigationEnum.Background, backgroundContainer }
            };
            
            for (var i = 0; i < iconItems.Count; ++i)
            {
                iconItems[i].Set(icons[i]);
            }
            
            for (var i = 0; i < backgroundImages.Count; ++i)
            {
                backgroundImages[i].Set(colors[i]);
            }
            
            _currentTab = TabsNavigationEnum.Icon;
            SetActiveTab();
        }

        private void SwitchTab(TabsNavigationEnum navigation)
        {
            if(_currentTab ==  navigation)
                return;

            foreach (var tab in _tabContents.Keys)
            {
                if (tab == navigation)
                {
                    _tabContents[_currentTab].gameObject.SetActive(false);
                    _currentTab = tab;
                    SetActiveTab();
                }
            }
        }

        private void SetActiveTab()
        {
            var container = _tabContents[_currentTab];
            container.SetActive(true);
        }
    }
}