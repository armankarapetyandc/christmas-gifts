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
    }
}