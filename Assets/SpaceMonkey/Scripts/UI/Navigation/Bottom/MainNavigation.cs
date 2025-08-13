using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using SpaceMonkey.Scripts.UI.Navigation.Core;
using SpaceMonkey.Scripts.UI.Views.BusinessHub;
using SpaceMonkey.Scripts.UI.Views.Map;
using SpaceMonkey.Scripts.UI.Views.Startup;
using UIService.Runtime.Core;
using UIService.Runtime.Utilities;
using UnityEngine;
using Logger = DCLogger.Runtime.Logger;

namespace SpaceMonkey.Scripts.UI.Navigation.Bottom
{
    public class MainNavigation : NavigationPresenterWithController<MainNavigationController>
    {
        public class Data : IPresenterData
        {
            public MainNavigationType Type { get; set; }
        }

        [SerializeField] private List<MainNavigationElement> elements;
        [SerializeField] private MainNavigationType defaultType;
        [SerializeField] private RectTransform baseHolder;


        private Data _data;
        private Dictionary<MainNavigationType, MainNavigationElement> _elementsDict = new();
        private readonly ReactiveProperty<MainNavigationType> _selected = new(MainNavigationType.None);

        public ReadOnlyReactiveProperty<MainNavigationType> Selected => _selected;


        public override UniTask Initialize(IPresenterData data = null)
        {
            _data = (Data)data;
            _selected.Value = defaultType;
            baseHolder.FitInSafeArea(FitmentType.Bottom);
            foreach (MainNavigationElement element in elements)
            {
                _elementsDict.Add(element.Type, element);
                element.OnSelectObservable
                    .Where(type => type != Selected.CurrentValue)
                    .Subscribe(_ =>
                    {
                        element.SelectWithoutNotification();
                        ShowView(element.Type).Forget();
                    })
                    .AddTo(this);
            }

            var navigateTo = _data?.Type ?? defaultType;
            ShowView(navigateTo).Forget();
            return UniTask.CompletedTask;
        }

        public void SelectNavigation(MainNavigationType type)
        {
            if (type == Selected.CurrentValue)
            {
                return;
            }

            var element = elements.FirstOrDefault(element => element.Type == type);
            if (element == null)
            {
                Logger.Log($"Unable to select {type} element!", UILogChannels.Error);
                return;
            }

            element.Select();
        }

        public void SelectDefaultPanel()
        {
            SelectNavigation(defaultType);
        }

        private async UniTaskVoid ShowView(MainNavigationType type)
        {
            await UniTask.Yield();
            _selected.Value = type;
            switch (type)
            {
                case MainNavigationType.Map:
                    Controller.HidePreviousAndShow<MapView>().Forget();
                    break;
                case MainNavigationType.BusinessHub:
                    Controller.HidePreviousAndShow<BusinessHubView>().Forget();
                    break;
                case MainNavigationType.Opportunities:
                    break;
                case MainNavigationType.Medal:
                    break;
                case MainNavigationType.More:
                    Controller.HidePreviousAndShow<StartupView>().Forget();
                    Controller.HideMainNavigation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}