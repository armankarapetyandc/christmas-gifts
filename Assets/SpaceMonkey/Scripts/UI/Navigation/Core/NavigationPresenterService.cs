using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceMonkey.Scripts.UI.Navigation.Core
{
    public class NavigationPresenterService
    {
        private const string PanelPrefabsPath = "UI/Presenters/Navigations";
        private readonly PresenterLoader _asset;
        private readonly ReactiveCommand<NavigationPresenter> _panelShowObservable;
        private readonly List<NavigationPresenter> _activePanels;

        public Observable<NavigationPresenter> PanelShowObservable => _panelShowObservable;

        public List<NavigationPresenter> ActivePanels => _activePanels;

        public NavigationPresenterService(PresenterLoader asset)
        {
            _asset = asset;
            _activePanels = new List<NavigationPresenter>();
            _panelShowObservable = new ReactiveCommand<NavigationPresenter>();
        }

        public async UniTask<T> Show<T>(IPresenterData data = null) where T : NavigationPresenter
        {
            var activePanel = _activePanels.FirstOrDefault(panel => panel is T) as T;
            if (activePanel != null)
            {
                return activePanel;
            }

            string id = Guid.NewGuid().ToString();

            T panelPrefab = await _asset.LoadPrefabAsync<T>(PanelPrefabsPath);
            T panel = Object.Instantiate(panelPrefab);
            panel.Disable();
            _activePanels.Add(panel);
            await panel.Initialize(data);
            _panelShowObservable.Execute(panel);
            panel.CallBeforeShow();
            panel.Enable();
            await panel.Show();
            panel.CallAfterShow();
            return panel;
        }


        public void Hide<T>() where T : NavigationPresenter
        {
            NavigationPresenter panel = _activePanels.FirstOrDefault(panel => panel is T);
            if (panel != null)
            {
                panel.Hide();
                _activePanels.Remove(panel);
                Object.Destroy(panel.gameObject);
            }
        }

        public void HideAll()
        {
            foreach (NavigationPresenter panel in _activePanels)
            {
                panel.Hide();
                Object.Destroy(panel.gameObject);
            }

            _activePanels.Clear();
        }

        public T GetPresenter<T>() where T : NavigationPresenter
        {
            NavigationPresenter panel = _activePanels.FirstOrDefault(panel => panel is T);
            if (panel == null)
            {
                Debug.LogError($"Unable to find panel{typeof(T)}");
            }

            return panel as T;
        }
    }
}