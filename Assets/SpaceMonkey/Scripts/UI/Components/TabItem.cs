using R3;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Components
{
    public class TabItem : MonoBehaviour
    {
        [SerializeField] private Sprite selectedStateSprite;
        [SerializeField] private Sprite deselectedStateSprite;
        [SerializeField] private Image image;
        [SerializeField] private Toggle toggle;
        [SerializeField] private TabContent content;

        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(OnStateChanged).AddTo(this);
        }

        private void OnStateChanged(bool state)
        {
            content.SetState(state);
            image.sprite = state ? selectedStateSprite : deselectedStateSprite;
        }
    }
}