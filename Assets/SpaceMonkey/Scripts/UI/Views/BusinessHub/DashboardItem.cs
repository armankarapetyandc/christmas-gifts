using System;
using R3;
using SpaceMonkey.Scripts.UI.Asset.Dashboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.BusinessHub
{
    public class DashboardItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private Button button;

        private DashboardItemAsset _itemAsset;
        
        public bool HasItem => _itemAsset != null;

        public Observable<DashboardItemAsset> SelectedItem => button.OnClickAsObservable().Select(_ => _itemAsset);

        internal void Set(DashboardItemAsset itemAsset)
        {
            _itemAsset = itemAsset;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_itemAsset == null)
            {
                throw new Exception("Dashboard item asset is null");
            }
            
            itemName.text = _itemAsset.DashboardName;
            icon.sprite = _itemAsset.DashboardItemSprite;
        }
    }
}