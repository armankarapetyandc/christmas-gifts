using System.Collections.Generic;
using SpaceMonkey.Scripts.UI.Views.ProductionCapacity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility
{
    public class ScrollSnap : MonoBehaviour,  IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Slider prodCapSlider;
        [SerializeField] private  float snapSpeed = 5f;
        [SerializeField] private Image background;
        
        private float[] _points;       
        private int _currentPage;
        private bool _isDragging;
        private float _velocity;
        private List<LevelItemComponent> _levelItems;
        
        public LevelItemComponent SelectedLevelItem => _levelItems[_currentPage];

        public void Initialize(List<LevelItemComponent> levelItems)
        {
            _levelItems = levelItems;
            _points = new float[_levelItems.Count];
            prodCapSlider.value = 0;
            
            float step = 1f / (levelItems.Count - 1);

            for (int i = 0; i < levelItems.Count; i++)
                _points[i] = step * i;
        }
        
        public void SnapToIndex(int index)
        {
            if (_points == null || _points.Length == 0)
                return;

            if (index < 0 || index >= _points.Length)
                return;

            _currentPage = index;

            scrollRect.horizontalNormalizedPosition = _points[_currentPage];
            prodCapSlider.value = _currentPage;
            background.color = _levelItems[_currentPage].BackgroundColor;

            _levelItems[_currentPage].ChangeState();
        }

        
        private void Update()
        {
            if (!_isDragging && _points.Length > 0)
            {
                scrollRect.horizontalNormalizedPosition =
                    Mathf.SmoothDamp(
                        scrollRect.horizontalNormalizedPosition,
                        _points[_currentPage],
                        ref _velocity,
                        1f / snapSpeed 
                    );
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;

            if (_points == null || _points.Length == 0)
                return;

            float pos = scrollRect.horizontalNormalizedPosition;
            float closestDistance = float.MaxValue;
            int closestPage = _currentPage;

            for (int i = 0; i < _points.Length; i++)
            {
                float distance = Mathf.Abs(_points[i] - pos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPage = i;
                }
            }

            if (closestPage != _currentPage)
            {
                _currentPage = closestPage;
                _levelItems[_currentPage].ChangeState();
                prodCapSlider.value = _currentPage;
                background.color = _levelItems[_currentPage].BackgroundColor;
            }
        }
    }
}