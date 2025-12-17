using System;
using Cysharp.Threading.Tasks;
using SpaceMonkey.Scripts.Tutorial.FunnySlide;
using SpaceMonkey.Scripts.UI.Utility;
using UnityEngine;

namespace SpaceMonkey.Scripts.Tutorial
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private FunnySlideOut funnySlideOut;
        [SerializeField] private CanvasGapRaycast canvasGapRaycast; 
        
        private Vector3 _funnySlideOutInitialPosition;

        private void Awake()
        {
            _funnySlideOutInitialPosition = funnySlideOut.transform.position;
        }

        public void ShowArrow(RectTransform rectTransform)
        {
            arrow.gameObject.SetActive(true);
            Vector2 pivot = rectTransform.pivot;

            Vector3 worldPoint = rectTransform.TransformPoint(new Vector3(
                rectTransform.rect.width * (0.5f - pivot.x),
                rectTransform.rect.height * (0.5f - pivot.y),
                0f
            ));

            arrow.position = worldPoint;
        }

        public void ShowFunnySlideOut(string description)
        {
            funnySlideOut.Show(description);
        }
        
        public void HideFunnySlideOut()
        {
            funnySlideOut.transform.position = _funnySlideOutInitialPosition;
            funnySlideOut.Hide();
        }

        public async void ShowMask(RectTransform rectTransform)
        {
            canvasGapRaycast.gameObject.SetActive(true);
            canvasGapRaycast.SetHoleFromUIElement(CanvasGapRaycast.HoleShape.Rectangle, rectTransform);
        }
        
        public void Reset()
        {
            HideArrow();
            HideMask();
            HideFunnySlideOut();
        }

        public void HideArrow()
        {
            arrow.gameObject.SetActive(false);
        }


        public void HideMask()
        {
            canvasGapRaycast.gameObject.SetActive(false);
        }

        public void AddArrowYOffset(float offset)
        {
            arrow.position += new Vector3(0, offset, 0);
        }

        public void SetFunnySlideOutPosition(Vector3 position)
        {
            funnySlideOut.transform.position = position;
        }

        public void AddFunnySlideOutYOffset(int i)
        {
            funnySlideOut.transform.position += new Vector3(0, i, 0);
        }
        
        public void AddFunnySlideOutXOffset(int i)
        {
            funnySlideOut.transform.position += new Vector3(i, 0, 0);
        }
    }
}