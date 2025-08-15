using R3;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Components.HashTag.Items
{
    public class HashtagListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;

        private readonly ReactiveProperty<bool> _isSelected = new ReactiveProperty<bool>(false);
        public Observable<bool> Selected => _isSelected;

#if UNITY_EDITOR
        private void OnValidate()
        {
            text = GetComponent<TextMeshProUGUI>();
            text.color = deselectedColor;
        }
#endif
        public void Set(string value)
        {
            text.text = value;
        }

        public void Select()
        {
            text.color = selectedColor;
            _isSelected.Value = true;
        }

        public void Deselect()
        {
            text.color = deselectedColor;
            _isSelected.Value = false;
        }
    }
}