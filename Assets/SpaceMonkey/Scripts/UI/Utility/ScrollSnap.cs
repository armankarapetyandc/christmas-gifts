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
        [SerializeField] private  float snapSpeed = 10f;
        [SerializeField] private Image background;
        
        private float[] _points;       
        private int _currentPage;
        private bool _isDragging;
        private List<LevelItemComponent> _levelItems;

        public void Initialize(List<LevelItemComponent> levelItems)
        {
            _levelItems = levelItems;
            _points = new float[_levelItems.Count];
            
            float step = 1f / (levelItems.Count - 1);

            for (int i = 0; i < levelItems.Count; i++)
                _points[i] = step * i;
        }
        
        private void Update()
        {
            if (!_isDragging && _points.Length > 0)
            {
                scrollRect.horizontalNormalizedPosition = 
                    Mathf.Lerp(scrollRect.horizontalNormalizedPosition, _points[_currentPage], Time.deltaTime * snapSpeed);
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
                background.color = _levelItems[_currentPage].BackgroundColor;
            }
        }
    }
}