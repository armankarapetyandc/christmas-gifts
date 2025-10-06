using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Utility
{
    public class Spoiler : MonoBehaviour
    {
        public RectTransform itemsPanel;
        public Button arrowButton;
        public float animationDuration = 0.3f;

        private bool _isOpen = true;

        private void Start()
        {
            arrowButton.OnClickAsObservable().Subscribe(_ => ToggleSpoiler()).AddTo(this);
            itemsPanel.localScale = new Vector3(1, 1, 1);
        }

        private async void ToggleSpoiler()
        {
            _isOpen = !_isOpen;

            float targetScaleY = _isOpen ? 1 : 0;


            await AnimateScale(itemsPanel, targetScaleY, animationDuration);

            arrowButton.transform.rotation = Quaternion.Euler(0, 0, _isOpen ? 0 : 180);
        }

        private async UniTask AnimateScale(RectTransform panel, float targetScaleY, float duration)
        {
            float startScaleY = panel.localScale.y;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float newY = Mathf.Lerp(startScaleY, targetScaleY, time / duration);
                panel.localScale = new Vector3(1, newY, 1);
                await UniTask.Yield(); // wait for next frame
            }

            panel.localScale = new Vector3(1, targetScaleY, 1); // ensure final value
        }
    }
}