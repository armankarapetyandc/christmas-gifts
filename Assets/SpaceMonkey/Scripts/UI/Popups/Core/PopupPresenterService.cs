using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UIService.Runtime.Core;
using UIService.Runtime.Utilities;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Popups.Core
{
    public class PopupPresenterService
    {
        private const string PanelPrefabsPath = "UI/Presenters/Popups";
        private readonly PresenterLoader _asset;
        private readonly ReactiveCommand<PopupPresenter> _panelShowObservable;
        private readonly List<PopupPresenter> _activePanels;

        public Observable<PopupPresenter> PanelShowObservable => _panelShowObservable;

        public List<PopupPresenter> ActivePanels => _activePanels;

        public PopupPresenterService(PresenterLoader asset)
        {
            _asset = asset;
            _activePanels = new List<PopupPresenter>();
            _panelShowObservable = new ReactiveCommand<PopupPresenter>();
        }

        public async UniTask<T> Show<T>(IPresenterData data = null) where T : PopupPresenter
        {
            T panelPrefab = await _asset.LoadPrefabAsync<T>(PanelPrefabsPath);
            T panel = Object.Instantiate(panelPrefab);
            _panelShowObservable.Execute(panel);
            _activePanels.Add(panel);
            panel.Initialize(data);
            panel.Show();

            Debug.Log("Show");
            return panel;
        }

        public void HideLast()
        {
            PopupPresenter panel = _activePanels[^1];
            panel.Hide();
            _activePanels.Remove(panel);
            Object.Destroy(panel.gameObject);
        }

        public void Hide<T>() where T : PopupPresenter
        {
            PopupPresenter panel = _activePanels.FirstOrDefault(panel => panel is T);
            if (panel != null)
            {
                panel.Hide();
                _activePanels.Remove(panel);
                Object.Destroy(panel.gameObject);
            }
        }

        public void HideAll()
        {
            foreach (PopupPresenter panel in _activePanels)
            {
                panel.Hide();
                Object.Destroy(panel.gameObject);
            }

            _activePanels.Clear();
        }

        public T GetPresenter<T>() where T : PopupPresenter
        {
            PopupPresenter panel = _activePanels.FirstOrDefault(panel => panel is T);
            if (panel == null)
            {
                Debug.LogError($"Unable to find panel{typeof(T)}");
            }

            return panel as T;
        }
    }
}