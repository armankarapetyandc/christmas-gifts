using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.BusinessExamples
{
    public class ExampleTypeTab : MonoBehaviour
    {
        [SerializeField] private string categoryName;
        [SerializeField] private Sprite selectedStateSprite;
        [SerializeField] private Sprite deselectedStateSprite;
        [SerializeField] private Image image;
        [SerializeField] internal Toggle toggle;
        
        public Observable<string> OnSelected =>
            toggle.OnValueChangedAsObservable().Where(b => b).Select(_ => categoryName);
        public string Category => categoryName;
        
        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(OnStateChanged).AddTo(this);
        }
        
        private void OnStateChanged(bool state)
        {
            image.sprite = state ? selectedStateSprite : deselectedStateSprite;
        }
    }
}