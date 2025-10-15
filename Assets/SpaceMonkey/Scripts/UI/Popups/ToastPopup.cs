using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Popups
{
    public class ToastPopup : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 50f;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float stayDuration = 3f;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private GameObject moneyIcon;
        
        
        
        private Vector2 initialPosition;
        private bool isShowing;

        void Awake()
        {
            initialPosition = rectTransform.anchoredPosition;
        }

        public async void ShowToast(string info)
        {
            if (isShowing) return;
            isShowing = true;
            moneyText.gameObject.SetActive(false);
            moneyIcon.SetActive(false);
            infoText.text = info;
            await AnimateToast();
            isShowing = false;
        }
        
        public async void ShowToast(string info,float money)
        {
            if (isShowing) return;
            isShowing = true;
            moneyText.gameObject.SetActive(true);
            moneyIcon.SetActive(true);
            infoText.text = info;
            moneyText.text = $"${money}";
            await AnimateToast();
            isShowing = false;
        }

        private async UniTask AnimateToast()
        {
            Vector2 startPos = initialPosition;
            Vector2 endPos = startPos + new Vector2(0, moveDistance);
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                canvasGroup.alpha = t;
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                await UniTask.Yield();
            }

            canvasGroup.alpha = 1;
            rectTransform.anchoredPosition = endPos;

            await UniTask.Delay(System.TimeSpan.FromSeconds(stayDuration));

            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                canvasGroup.alpha = 1 - t;
                await UniTask.Yield();
            }

            canvasGroup.alpha = 0;
            rectTransform.anchoredPosition = startPos;
        }
    }
}