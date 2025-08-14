using R3;
using TMPro;
using UnityEngine;

namespace SpaceMonkey.Scripts.UI.Views.Business.BusinessSetup.Panels.BusinessDetails.Items
{
    public class HashtagListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;

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
        }

        public void Deselect()
        {
            text.color = deselectedColor;
        }
    }
}