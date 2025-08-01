using System;
using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.UI.Asset;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.CategorySelection
{
    public class CategoryItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        private CategoryInfo _categoryInfo;

        public Observable<CategoryInfo> Selected => button.OnClickAsObservable().Select(_ => _categoryInfo);

        public bool HasIdea => _categoryInfo != null;
        
        internal void Set(CategoryInfo categoryInfo)
        {
            _categoryInfo = categoryInfo;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_categoryInfo == null)
            {
                return;
            }

            titleText.text = _categoryInfo.Name;
            iconImage.sprite = _categoryInfo.Visual.Sprite;
        }
    }
}