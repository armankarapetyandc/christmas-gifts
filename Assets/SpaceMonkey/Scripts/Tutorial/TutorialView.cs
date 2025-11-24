using DreamCode.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.Tutorial
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private Image maskImage;
        [SerializeField] private Image blocker;
        [SerializeField] private MaskInverter maskInverter;
        
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

        public void ShowMask(RectTransform rectTransform)
        {
            var image = rectTransform.GetComponent<Image>() ?? rectTransform.GetComponentInChildren<Image>(true);
            var material = maskInverter.GetModifiedMaterial(image.material);
            
            blocker.material = material;
            blocker.gameObject.SetActive(true);
            
            maskImage.sprite = image.sprite;
            maskImage.SetNativeSize();
            maskImage.gameObject.SetActive(true);
            
            Vector2 pivot = rectTransform.pivot;
            Vector3 worldPoint = rectTransform.TransformPoint(new Vector3(
                rectTransform.rect.width * (0.5f - pivot.x),
                rectTransform.rect.height * (0.5f - pivot.y),
                0f
            ));
            maskImage.transform.position = worldPoint;
        }
        
        public void Reset()
        {
            arrow.gameObject.SetActive(false);
        }
    }
}