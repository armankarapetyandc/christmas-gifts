using R3;
using SpaceMonkey.Scripts.Configs;
using SpaceMonkey.Scripts.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.HashTag.Items
{
    [ExecuteAlways]
    public class HashTagItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Sprite selectedStateSprite;
        [SerializeField] private Sprite deselectedStateSprite;
        [SerializeField] private TextMeshProUGUI text;
        private HashtagInfo _hashtag;
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public LayoutElement LayoutElement { get; private set; }

        public Observable<(Hashtag, bool)> OnValueChanged =>
            toggle.OnValueChangedAsObservable().Select(state => (Hashtag.FromHashtagInfo(_hashtag), state));

        private void Start()
        {
            toggle.OnValueChangedAsObservable().Subscribe(UpdateStateUI).AddTo(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
            LayoutElement ??= GetComponent<LayoutElement>();
            UpdateStateUI(toggle.isOn);
        }

        private void LateUpdate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            UpdateStateUI(toggle.isOn);
        }
#endif

        private void UpdateStateUI(bool state)
        {
            image.sprite = state ? selectedStateSprite : deselectedStateSprite;
        }

        public void SetText(HashtagInfo hashtag)
        {
            _hashtag = hashtag;
            text.text = hashtag.Tag;
        }
    }
}